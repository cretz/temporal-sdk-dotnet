namespace Temporalio.Tests.Worker;

using NexusRpc;
using NexusRpc.Handler;
using Temporalio.Api.Enums.V1;
using Temporalio.Client;
using Temporalio.Nexus;
using Temporalio.Worker;
using Temporalio.Workflows;
using Xunit;
using Xunit.Abstractions;

public class NexusWorkerTests : WorkflowEnvironmentTestBase
{
    public NexusWorkerTests(ITestOutputHelper output, WorkflowEnvironment env)
        : base(output, env)
    {
    }

    [NexusService]
    public interface ISimpleService
    {
        [NexusOperation]
        string SayHello(string name);
    }

    [NexusServiceHandler(typeof(ISimpleService))]
    public class SimpleService
    {
        [NexusOperationHandler]
        public IOperationHandler<string, string> SayHello() =>
            OperationHandler.Sync<string, string>((ctx, name) => $"Hello, {name}");
    }

    [Workflow]
    public class SimpleServiceWorkflow
    {
        [WorkflowRun]
        public Task<string> RunAsync(string endpoint, string name) =>
            Workflow.CreateNexusClient<ISimpleService>(endpoint).
                ExecuteNexusOperationAsync(svc => svc.SayHello(name));
    }

    [Fact]
    public async Task ExecuteNexusOperationAsync_SimpleService_Succeeds()
    {
        var taskQueue = $"tq-{Guid.NewGuid()}";
        var endpoint = await CreateNexusEndpointAsync(taskQueue);
        using var worker = new TemporalWorker(
            Client,
            new TemporalWorkerOptions(taskQueue).
                AddWorkflow<SimpleServiceWorkflow>().
                AddNexusService(new SimpleService()));
        await worker.ExecuteAsync(async () =>
        {
            var result = await Client.ExecuteWorkflowAsync(
                (SimpleServiceWorkflow wf) => wf.RunAsync(endpoint, "some-name"),
                new($"wf-{Guid.NewGuid()}", taskQueue));
            Assert.Equal("Hello, some-name", result);
        });
    }

    [NexusService]
    public interface IWorkflowBackedService
    {
        [NexusOperation]
        string SayHello(string name);
    }

    [NexusServiceHandler(typeof(IWorkflowBackedService))]
    public class WorkflowBackedService
    {
        // [NexusOperationHandler]
        // public IOperationHandler<string, string> SayHello() =>
        //     WorkflowRunOperationHandler.FromWorkflowRunDelegate3(
        //         (SimpleWorkflow wf, string input) => wf.RunAsync(input));
    }

    [Workflow]
    public class SimpleWorkflow
    {
        [WorkflowRun]
        public async Task<string> RunAsync(string name) => $"Hello, {name}";
    }

    [Fact]
    public void Temp()
    {

// Above is just sugar for (so do we need the above form)
var handler2 = WorkflowRunOperationHandler.FromHandleFactory(
    (WorkflowRunOperationContext context, string input) =>
        context.StartWorkflowAsync((SimpleWorkflow wf) => wf.RunAsync(input)));

// Need to adjust input (showing use of type args instead of param types)
var handler3 = WorkflowRunOperationHandler.FromHandleFactory<string, string>(
    (context, input) =>
    {
        var newInput = input + "-with-suffix";
        return context.StartWorkflowAsync((SimpleWorkflow wf) => wf.RunAsync(newInput));
    });

// Same as above, but using param types instead of type args
var handler4 = WorkflowRunOperationHandler.FromHandleFactory(
    (WorkflowRunOperationContext context, string input) =>
    {
        var newInput = input + "-with-suffix";
        return context.StartWorkflowAsync((SimpleWorkflow wf) => wf.RunAsync(newInput));
    });

// String-name based workflow
var handler5 = WorkflowRunOperationHandler.FromHandleFactory(
    (WorkflowRunOperationContext context, string input) =>
        // Note a return type has to be manually set here
        context.StartWorkflowAsync<string>("SimpleWorkflow", new object[] { input }));

// Need to set something like conflict type option
var handler6 = WorkflowRunOperationHandler.FromHandleFactory(
    (WorkflowRunOperationContext context, string input) =>
        context.StartWorkflowAsync(
            (SimpleWorkflow wf) => wf.RunAsync(input),
            new() { IdConflictPolicy = WorkflowIdConflictPolicy.UseExisting }));
    }

    private async Task<string> CreateNexusEndpointAsync(string taskQueue)
    {
        var name = $"nexus-endpoint-{taskQueue}";
        await Client.OperatorService.CreateNexusEndpointAsync(new()
        {
            Spec = new()
            {
                Name = name,
                Target = new()
                {
                    Worker = new() { Namespace = Client.Options.Namespace, TaskQueue = taskQueue },
                },
            },
        });
        return name;
    }

    /*
    TODO:

    * Register bad handler on worker
    * Sync result timeout
    * Async result cancel
    * Workflow-based async result

    */
}