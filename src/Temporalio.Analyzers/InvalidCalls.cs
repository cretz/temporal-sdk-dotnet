using System.Collections;
using System.Collections.Generic;

namespace Temporalio.Analyzers
{
    public class InvalidCalls
    {
        // Keyed by type, method, then string if invalid or no string if valid
        private readonly Dictionary<string, Dictionary<string, string?>> invalid;

        public InvalidCalls(Dictionary<string, Dictionary<string, string?>> invalid) =>
            this.invalid = invalid;

        public bool TryGetInvalid(string type, string method, out string? isInvalid)
        {
            if (invalid.TryGetValue(type, out var methods) &&
                methods.TryGetValue(method, out isInvalid))
            {
                return true;
            }
            isInvalid = null;
            return false;
        }
    }
}