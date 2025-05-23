namespace Temporalio.Worker.Tuning
{
    /// <summary>
    /// WorkerTuners allow for the dynamic customization of some aspects of worker configuration.
    /// </summary>
    public interface IWorkerTuner
    {
        /// <summary>
        /// Gets a slot supplier for workflow tasks.
        /// </summary>
        /// <returns>A slot supplier for workflow tasks.</returns>
        SlotSupplier WorkflowTaskSlotSupplier { get; }

        /// <summary>
        /// Gets a slot supplier for activity tasks.
        /// </summary>
        /// <returns>A slot supplier for activity tasks.</returns>
        SlotSupplier ActivityTaskSlotSupplier { get; }

        /// <summary>
        /// Gets a slot supplier for local activities.
        /// </summary>
        /// <returns>A slot supplier for local activities.</returns>
        SlotSupplier LocalActivitySlotSupplier { get; }
    }
}
