using System;
using NexusRpc;
using Temporalio.Converters;

namespace Temporalio.Worker
{
    internal class NexusPayloadSerializer : ISerializer
    {
        public NexusPayloadSerializer(DataConverter dataConverter)
        {
            throw new NotImplementedException("TODO");
        }

        public ISerializer.Content Serialize(object? value) => throw new NotImplementedException();

        public object? Deserialize(ISerializer.Content content, Type type) => throw new NotImplementedException();
    }
}