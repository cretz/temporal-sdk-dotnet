using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Temporalio.Nexus
{
    public class WorkflowRunHandle<TResult>
    {
        internal WorkflowRunHandle(
            string namespace_,
            string workflowId,
            int version = 0)
        {
            Namespace = namespace_;
            WorkflowId = workflowId;
            Version = version;
        }

        internal string Namespace { get; private init; }

        internal string WorkflowId { get; private init; }

        internal int Version { get; private init; }

        internal string ToToken() => JsonSerializer.Serialize(new Token(
            Namespace,
            WorkflowId,
            Version == 0 ? null : Version));

        internal static WorkflowRunHandle<TResult> FromToken(string token)
        {
            var tokenObj = JsonSerializer.Deserialize<Token>(token) ??
                throw new InvalidOperationException("Token invalid");
            if (tokenObj.Version != null && tokenObj.Version != 0)
            {
                throw new InvalidOperationException($"Unsupported token version: {tokenObj.Version}");
            }
            return new(tokenObj.Namespace, tokenObj.WorkflowId, tokenObj.Version ?? 0);
        }

        private record Token(
            [property: JsonPropertyName("ns")]
            string Namespace,
            [property: JsonPropertyName("wid")]
            string WorkflowId,
            [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            [property: JsonPropertyName("v")]
            int? Version,
            [property: JsonPropertyName("t'")]
            int Type = 1);
    }
}