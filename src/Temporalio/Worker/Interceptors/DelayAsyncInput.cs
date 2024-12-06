using System;
using System.Threading;
using Temporalio.Workflows;

namespace Temporalio.Worker.Interceptors
{
    /// <summary>
    /// Input for <see cref="WorkflowOutboundInterceptor.DelayAsync" />.
    /// </summary>
    /// <param name="Delay">Delay duration.</param>
    /// <param name="CancellationToken">Optional cancellation token.</param>
    /// <param name="Options">Options for the delay.</param>
    /// <remarks>
    /// WARNING: This constructor may have required properties added. Do not rely on the exact
    /// constructor, only use "with" clauses.
    /// </remarks>
    public record DelayAsyncInput(
        TimeSpan Delay,
        CancellationToken? CancellationToken,
        DelayOptions Options);
}