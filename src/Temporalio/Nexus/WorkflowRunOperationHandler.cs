using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NexusRpc;
using NexusRpc.Handler;
using Temporalio.Client;

namespace Temporalio.Nexus
{
    public static class WorkflowRunOperationHandler
    {
        public static IOperationHandler<TInput, TResult> FromHandleFactory<TInput, TResult>(
            HandleFactory<TInput, TResult> handleFactory)
        {
            throw new NotImplementedException();
        }

        public delegate Task<WorkflowRunHandle<TResult>> HandleFactory<TInput, TResult>(
            WorkflowRunOperationContext context, TInput input);
    }

    internal class WorkflowRunOperationHandler<TInput, TResult> : IOperationHandler<TInput, TResult>
    {
        private readonly WorkflowRunOperationHandler.HandleFactory<TInput, TResult> handleFactory;

        internal WorkflowRunOperationHandler(
            WorkflowRunOperationHandler.HandleFactory<TInput, TResult> handleFactory) =>
            this.handleFactory = handleFactory;

        public async Task<OperationStartResult<TResult>> StartAsync(
            OperationStartContext context, TInput input)
        {
            var handle = await handleFactory(new(context), input).ConfigureAwait(false);
            return OperationStartResult.AsyncResult<TResult>(handle.ToToken());
        }

        public Task<TResult> FetchResultAsync(OperationFetchResultContext context) =>
            throw new NotImplementedException();

        public Task<OperationInfo> FetchInfoAsync(OperationFetchInfoContext context) =>
            throw new NotImplementedException();

        public Task CancelAsync(OperationCancelContext context)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}