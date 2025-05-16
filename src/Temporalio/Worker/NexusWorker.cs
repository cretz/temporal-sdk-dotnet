using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using NexusRpc;
using NexusRpc.Handler;
using Temporalio.Api.Common.V1;
using Temporalio.Api.Nexus.V1;
using Temporalio.Api.WorkflowService.V1;
using Temporalio.Bridge.Api.Nexus;
using Temporalio.Converters;
using Temporalio.Exceptions;

namespace Temporalio.Worker
{
    internal class NexusWorker
    {
        private readonly TemporalWorker worker;
        private readonly ILogger logger;
        private readonly Handler handler;
        private readonly ConcurrentDictionary<ByteString, RunningTask> runningTasks = new();

        public NexusWorker(TemporalWorker worker)
        {
            this.worker = worker;
            logger = worker.LoggerFactory.CreateLogger<NexusWorker>();
            var instances = new Dictionary<string, ServiceHandlerInstance>(
                worker.Options.NexusServices.Count);
            foreach (var instance in worker.Options.NexusServices)
            {
                if (instances.ContainsKey(instance.Definition.Name))
                {
                    throw new ArgumentException($"Duplicate Nexus service named {instance.Definition.Name}");
                }
                instances[instance.Definition.Name] = instance;
            }
            handler = new Handler(
                instances,
                new NexusPayloadSerializer(worker.Client.Options.DataConverter),
                // TODO(cretz): Interceptors
                Array.Empty<IOperationMiddleware>());
        }

        public async Task ExecuteAsync()
        {
            // Run poll loop until there is no poll left
            using (logger.BeginScope(new Dictionary<string, object>()
            {
                ["TaskQueue"] = worker.Options.TaskQueue!,
            }))
            {
                while (true)
                {
                    var task = await worker.BridgeWorker.PollNexusTaskAsync().ConfigureAwait(false);
                    logger.LogTrace("Received Nexus task: {Task}", task);
                    switch (task?.VariantCase)
                    {
                        case NexusTask.VariantOneofCase.Task:
                            // We know that the .NET task for the running task is accessed only on
                            // graceful shutdown which could never run until after this (and the
                            // primary execute) are done. So we don't have to worry about the
                            // dictionary having a running task without a .NET task even though
                            // we're late-binding it here.
                            var running = new RunningTask();
                            runningTasks[task.Task.TaskToken] = running;
#pragma warning disable CA2008 // We don't have to pass a scheduler, factory already implies one
                            running.Task = worker.Options.NexusTaskFactory.StartNew(
                                () => HandlePollTaskAsync(running, task.Task)).Unwrap();
#pragma warning restore CA2008
                            break;
                        case NexusTask.VariantOneofCase.CancelTask:
                            if (runningTasks.TryGetValue(task.CancelTask.TaskToken, out var toCancel))
                            {
                                try
                                {
                                    toCancel.Cancel(task.CancelTask);
                                }
#pragma warning disable CA1031 // We're ok catching all exceptions here
                                catch (Exception e)
#pragma warning restore CA1031
                                {
                                    // Log and swallow any cancellation callback exceptions
                                    logger.LogError(e, "Cancelling task failed");
                                }
                            }
                            // NOTE - we do not send an ack-cancel here even though Core allows it
                            // because cancellation is a request of the handler task and it is up
                            // to the user code to react to it or not.
                            break;
                        case null:
                            // This means worker shut down
                            return;
                        default:
                            throw new InvalidOperationException($"Unexpected Nexus task case {task?.VariantCase}");
                    }
                }
            }
        }

        private async Task HandlePollTaskAsync(RunningTask running, PollNexusTaskQueueResponse task)
        {
            try
            {
                // Handle poll and post back to Core
                var completion = await HandlePollTaskInternalAsync(running, task).ConfigureAwait(false);
                await worker.BridgeWorker.CompleteNexusTaskAsync(completion).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // We're ok catching all exceptions here
            catch (Exception e)
#pragma warning restore CA1031
            {
                // Failure completing in Core
                logger.LogError(e, "Unexpected error completing Nexus {OperationType} task", task.Request.VariantCase);
            }
            finally
            {
                // Make sure to remove running task when done
                runningTasks.TryRemove(task.TaskToken, out _);
            }
        }

        private async Task<NexusTaskCompletion> HandlePollTaskInternalAsync(
            RunningTask running, PollNexusTaskQueueResponse task)
        {
            try
            {
                // Handle each case
                switch (task.Request.VariantCase)
                {
                    case Request.VariantOneofCase.StartOperation:
                        var startResp = await HandleStartOperationAsync(running, task).ConfigureAwait(false);
                        return new()
                        {
                            TaskToken = task.TaskToken,
                            Completed = new() { StartOperation = startResp },
                        };
                    case Request.VariantOneofCase.CancelOperation:
                        var cancelResp = await HandleCancelOperationAsync(running, task).ConfigureAwait(false);
                        return new()
                        {
                            TaskToken = task.TaskToken,
                            Completed = new() { CancelOperation = cancelResp },
                        };
                    default:
                        throw new InvalidOperationException($"Unexpected Nexus request case {task.Request.VariantCase}");
                }
            }
#pragma warning disable CA1031 // We're ok catching all exceptions here
            catch (Exception e)
#pragma warning restore CA1031
            {
                logger.LogWarning(e, "Completing Nexus {OperationType} task as failed", task.Request.VariantCase);
                return new()
                {
                    TaskToken = task.TaskToken,
                    Error = new()
                    {
                        ErrorType = (e as HandlerException)?.RawErrorType ?? "INTERNAL",
                        Failure = await ExceptionToNexusFailureAsync(e).ConfigureAwait(false),
                    },
                };
            }
        }

        private async Task<StartOperationResponse> HandleStartOperationAsync(
            RunningTask running, PollNexusTaskQueueResponse task)
        {
            // Create context
            var startOp = task.Request.StartOperation;
            var context = new OperationStartContext(
                Service: startOp.Service,
                Operation: startOp.Operation,
                CancellationToken: running.CancellationTokenSource.Token,
                RequestId: startOp.RequestId)
            {
                Headers = task.Request.Header,
                CallbackUrl = string.IsNullOrEmpty(startOp.Callback) ? null : startOp.Callback,
                CallbackHeaders = startOp.CallbackHeader.Count == 0 ? null :
                    new Dictionary<string, string>(startOp.CallbackHeader, StringComparer.OrdinalIgnoreCase),
                InboundLinks = startOp.Links.Select(l =>
                {
                    try
                    {
                        return new NexusLink(new Uri(l.Url), l.Type);
                    }
                    catch (UriFormatException e)
                    {
                        throw new HandlerException(
                            HandlerErrorType.BadRequest,
                            $"Invalid link URL: {l.Url}",
                            e);
                    }
                }).ToList(),
            };

            // Start operation
            try
            {
                var result = await handler.StartOperationAsync(
                    context,
                    new HandlerContent(startOp.Payload.ToByteArray())).ConfigureAwait(false);
                var links = context.OutboundLinks.Select(l =>
                    new Api.Nexus.V1.Link() { Type = l.Type, Url = l.Uri.ToString(), });
                if (result.AsyncOperationToken is { } asyncOperationToken)
                {
                    return new()
                    {
                        AsyncSuccess = new()
                        {
                            OperationId = asyncOperationToken,
                            OperationToken = asyncOperationToken,
                            Links = { links },
                        },
                    };
                }
                return new()
                {
                    SyncSuccess = new()
                    {
                        Payload = Payload.Parser.ParseFrom(result.SyncResultValue!.ConsumeAllBytes()),
                        Links = { links },
                    },
                };
            }
            catch (OperationException e)
            {
                return new()
                {
                    OperationError = new()
                    {
                        OperationState = e.State.ToString().ToLower(),
                        Failure = await ExceptionToNexusFailureAsync(e).ConfigureAwait(false),
                    },
                };
            }
            catch (WorkflowFailedException e)
            {
                throw new HandlerException(HandlerErrorType.BadRequest, "Workflow failed", e);
            }
            catch (ApplicationFailureException e) when (e.NonRetryable)
            {
                throw new HandlerException(
                    HandlerErrorType.Internal,
                    "Handler failed with non-retryable application exception",
                    HandlerErrorRetryBehavior.NonRetryable);
            }
        }

        private async Task<CancelOperationResponse> HandleCancelOperationAsync(
            RunningTask running, PollNexusTaskQueueResponse task)
        {
            // Create context
            var cancelOp = task.Request.CancelOperation;
            var context = new OperationCancelContext(
                Service: cancelOp.Service,
                Operation: cancelOp.Operation,
                CancellationToken: running.CancellationTokenSource.Token,
                OperationToken: cancelOp.OperationToken)
            {
                Headers = task.Request.Header,
            };

            // Cancel operation
            await handler.CancelOperationAsync(context).ConfigureAwait(false);
            return new();
        }

        private async Task<Failure> ExceptionToNexusFailureAsync(Exception exc)
        {
            // Convert to failure, then capture message, then remove message and capture rest of
            // failure as proto JSON
            Api.Failure.V1.Failure failureProto;
            try
            {
                failureProto = await worker.Client.Options.DataConverter.ToFailureAsync(exc).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // We're ok catching all exceptions here
            catch (Exception e)
#pragma warning restore CA1031
            {
                logger.LogError(e, "Failure converting existing failure");
                failureProto = new() { Message = $"Failure converting existing failure: {e.Message}" };
            }
            // Capture message, then remove and serialize rest of failure as proto JSON
            var message = failureProto.Message;
            failureProto.Message = string.Empty;
            return new()
            {
                Message = message,
                Details = ByteString.CopyFromUtf8(JsonFormatter.Default.Format(failureProto)),
                Metadata = { ["type"] = Api.Failure.V1.Failure.Descriptor.FullName },
            };
        }

        private class RunningTask
        {
            ~RunningTask() => CancellationTokenSource.Dispose();

            public Task? Task { get; set; }

            public CancellationTokenSource CancellationTokenSource { get; } = new();

            public string? CancellationReason { get; set; }

            public void Cancel(CancelNexusTask task)
            {
                switch (task.Reason)
                {
                    case NexusTaskCancelReason.TimedOut:
                        CancellationReason ??= "timed out";
                        break;
                    case NexusTaskCancelReason.WorkerShutdown:
                        CancellationReason ??= "worker shutdown";
                        break;
                    default:
                        CancellationReason ??= task.Reason.ToString();
                        break;
                }
                CancellationTokenSource.Cancel();
            }
        }
    }
}