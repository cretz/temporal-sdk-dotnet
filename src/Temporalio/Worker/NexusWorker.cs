using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using NexusRpc;
using NexusRpc.Handler;
using Temporalio.Api.Nexus.V1;
using Temporalio.Api.WorkflowService.V1;
using Temporalio.Bridge.Api.Nexus;
using Temporalio.Exceptions;

namespace Temporalio.Worker
{
    internal class NexusWorker : IDisposable
    {
        private readonly TemporalWorker worker;
        private readonly ILogger logger;
        private readonly Handler handler;
        private readonly ConcurrentDictionary<ByteString, RunningTask> runningTasks = new();

        public NexusWorker(TemporalWorker worker)
        {
            this.worker = worker;
            logger = worker.LoggerFactory.CreateLogger<ActivityWorker>();
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
                        case Bridge.Api.Nexus.NexusTask.VariantOneofCase.Task:
                            // We know that the .NET task for the running task is accessed only on
                            // graceful shutdown which could never run until after this (and the
                            // primary execute) are done. So we don't have to worry about the
                            // dictionary having a running task without a .NET task even though
                            // we're late-binding it here.
                            var running = new RunningTask();
                            runningTasks[task.Task.TaskToken] = running;
#pragma warning disable CA2008 // We don't have to pass a scheduler, factory already implies one
                            running.Task = worker.Options.NexusTaskFactory.StartNew(
                                () => HandleTaskAsync(running, task.Task)).Unwrap();
#pragma warning restore CA2008
                            break;
                        case Bridge.Api.Nexus.NexusTask.VariantOneofCase.CancelTask:
                            // TODO
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

        private async Task HandleTaskAsync(RunningTask running, PollNexusTaskQueueResponse task)
        {
            // TODO(cretz): Try/catch
            // TODO(cretz): Timeout

            switch (task.Request.VariantCase)
            {
                case Api.Nexus.V1.Request.VariantOneofCase.StartOperation:
                    break;
                case Api.Nexus.V1.Request.VariantOneofCase.CancelOperation:
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected Nexus request case {task.Request.VariantCase}");
            }
        }

        private async Task<StartOperationResponse> HandleStartTaskAsync(
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
                CallbackHeaders = startOp.CallbackHeader.Count == 0 ? null : startOp.CallbackHeader,
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

            // Run operation
            var response = new StartOperationResponse();
            try
            {
                var result = await handler.StartOperationAsync(
                    context,
                    new HandlerContent(startOp.Payload.ToByteArray())).ConfigureAwait(false);
            }
            catch (OperationException e)
            {

            }
            catch (WorkflowFailedException e)
            {

            }
            catch (ApplicationFailureException e)
            {

            }

            // var completion = new NexusTaskCompletion()
            // {
            //     TaskToken = task.TaskToken,
            //     Completed = new()
            //     {
            //         StartOperation = new()
            //         {

            //         }
            //     }
            // }
        }

        private class RunningTask : IDisposable
        {
            public Task? Task { get; set; }

            public CancellationTokenSource CancellationTokenSource { get; } = new();

            public void Dispose() => CancellationTokenSource.Dispose();
        }
    }
}