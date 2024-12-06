using System;

namespace Temporalio.Workflows
{
    public class DelayOptions : ICloneable
    {
        public string? Summary { get; set; }

        /// <summary>
        /// Create a shallow copy of these options.
        /// </summary>
        /// <returns>A shallow copy of these options.</returns>
        public virtual object Clone() => MemberwiseClone();
    }
}
