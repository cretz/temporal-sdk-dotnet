using System.Threading.Tasks;
using Temporalio.Api.WorkflowService.V1;
using Temporalio.Converters;

namespace Temporalio.Client
{
    /// <summary>
    /// Representation of a workflow execution and description.
    /// </summary>
    public class WorkflowExecutionDescription : WorkflowExecution
    {
        private WorkflowExecutionDescription(
            DescribeWorkflowExecutionResponse rawDescription,
            string? staticSummary,
            string? staticDetails,
            DataConverter dataConverter)
            : base(rawDescription.WorkflowExecutionInfo, dataConverter)
        {
            RawDescription = rawDescription;
            StaticSummary = staticSummary;
            StaticDetails = staticDetails;
        }

        public string? StaticSummary { get; private init; }

        public string? StaticDetails { get; private init; }

        /// <summary>
        /// Gets the raw proto info.
        /// </summary>
        internal DescribeWorkflowExecutionResponse RawDescription { get; private init; }

        internal static async Task<WorkflowExecutionDescription> FromProtoAsync(
            DescribeWorkflowExecutionResponse rawDescription, DataConverter dataConverter)
        {
            var (staticSummary, staticDetails) = await dataConverter.FromUserMetadataAsync(
                rawDescription.ExecutionConfig?.UserMetadata).ConfigureAwait(false);
            return new(rawDescription, staticSummary, staticDetails, dataConverter);
        }
    }
}