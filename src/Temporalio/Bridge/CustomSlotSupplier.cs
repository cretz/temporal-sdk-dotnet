using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Temporalio.Bridge
{
    /// <summary>
    /// Core wrapper for a user-defined custom slot supplier.
    /// </summary>
    internal class CustomSlotSupplier : NativeInvokeableClass<Interop.TemporalCoreCustomSlotSupplierCallbacks>
    {
        private readonly ILogger logger;
        private readonly Temporalio.Worker.Tuning.CustomSlotSupplier userSupplier;
        private readonly Dictionary<uint, Temporalio.Worker.Tuning.SlotPermit> permits = new();
        private uint permitId = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSlotSupplier" /> class.
        /// </summary>
        /// <param name="userSupplier">User's slot supplier implementation'.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        internal unsafe CustomSlotSupplier(
            Temporalio.Worker.Tuning.CustomSlotSupplier userSupplier,
            ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<CustomSlotSupplier>();
            this.userSupplier = userSupplier;

            var interopCallbacks = new Interop.TemporalCoreCustomSlotSupplierCallbacks
            {
                reserve = FunctionPointer<Interop.TemporalCoreCustomReserveSlotCallback>(Reserve),
                cancel_reserve = FunctionPointer<Interop.TemporalCoreCustomCancelReserveCallback>(CancelReserve),
                try_reserve = FunctionPointer<Interop.TemporalCoreCustomTryReserveSlotCallback>(TryReserve),
                mark_used = FunctionPointer<Interop.TemporalCoreCustomMarkSlotUsedCallback>(MarkUsed),
                release = FunctionPointer<Interop.TemporalCoreCustomReleaseSlotCallback>(Release),
                free = FunctionPointer<Interop.TemporalCoreCustomSlotImplFreeCallback>(Free),
            };

            PinCallbackHolder(interopCallbacks);
        }

        private static Temporalio.Worker.Tuning.SlotInfo SlotInfoFromBridge(Interop.TemporalCoreSlotInfo slotInfo)
        {
            return slotInfo.tag switch
            {
                Interop.TemporalCoreSlotInfo_Tag.WorkflowSlotInfo =>
                    new Temporalio.Worker.Tuning.SlotInfo.WorkflowSlotInfo(
                        ByteArrayRef.ToUtf8(slotInfo.workflow_slot_info.workflow_type), slotInfo.workflow_slot_info.is_sticky != 0),
                Interop.TemporalCoreSlotInfo_Tag.ActivitySlotInfo =>
                    new Temporalio.Worker.Tuning.SlotInfo.ActivitySlotInfo(
                        ByteArrayRef.ToUtf8(slotInfo.activity_slot_info.activity_type)),
                Interop.TemporalCoreSlotInfo_Tag.LocalActivitySlotInfo =>
                    new Temporalio.Worker.Tuning.SlotInfo.LocalActivitySlotInfo(
                        ByteArrayRef.ToUtf8(slotInfo.local_activity_slot_info.activity_type)),
                _ => throw new System.ArgumentOutOfRangeException(nameof(slotInfo)),
            };
        }

        private unsafe void Reserve(Interop.TemporalCoreSlotReserveCtx* ctx, void* sender)
        {
            SafeReserve(new IntPtr(ctx), new IntPtr(sender));
        }

        // Note that this is always called by Rust, either because the call is cancelled or because
        // it completed. Therefore the GCHandle is always freed.
        private unsafe void CancelReserve(void* tokenSrc)
        {
            var handle = GCHandle.FromIntPtr(new IntPtr(tokenSrc));
            var cancelTokenSrc = (CancellationTokenSource)handle.Target!;
            cancelTokenSrc.Cancel();
            handle.Free();
        }

        private void SafeReserve(IntPtr ctx, IntPtr sender)
        {
            _ = Task.Run(async () =>
            {
                using (var cancelTokenSrc = new System.Threading.CancellationTokenSource())
                {
                    unsafe
                    {
                        var srcHandle = GCHandle.Alloc(cancelTokenSrc);
                        Interop.Methods.temporal_core_set_reserve_cancel_target(
                            (Interop.TemporalCoreSlotReserveCtx*)ctx.ToPointer(),
                            GCHandle.ToIntPtr(srcHandle).ToPointer());
                    }
                    while (true)
                    {
                        try
                        {
                            Task<Temporalio.Worker.Tuning.SlotPermit> reserveTask;
                            unsafe
                            {
                                reserveTask = userSupplier.ReserveSlotAsync(
                                    ReserveCtxFromBridge((Interop.TemporalCoreSlotReserveCtx*)ctx.ToPointer()),
                                    cancelTokenSrc.Token);
                            }
                            var permit = await reserveTask.ConfigureAwait(false);
                            unsafe
                            {
                                var usedPermitId = AddPermitToMap(permit);
                                Interop.Methods.temporal_core_complete_async_reserve(sender.ToPointer(), new(usedPermitId));
                            }
                            return;
                        }
                        catch (OperationCanceledException) when (cancelTokenSrc.Token.IsCancellationRequested)
                        {
                            unsafe
                            {
                                // Always call this to ensure the sender is freed
                                Interop.Methods.temporal_core_complete_async_reserve(sender.ToPointer(), new(0));
                            }
                            return;
                        }
#pragma warning disable CA1031 // We are ok catching all exceptions here
                        catch (Exception e)
                        {
#pragma warning restore CA1031
                            logger.LogError(e, "Error reserving slot");
                        }
                        // Wait for a bit to avoid spamming errors
                        await Task.Delay(1000, cancelTokenSrc.Token).ConfigureAwait(false);
                    }
                }
            });
        }

        private unsafe UIntPtr TryReserve(Interop.TemporalCoreSlotReserveCtx* ctx)
        {
            Temporalio.Worker.Tuning.SlotPermit? maybePermit;
            try
            {
                maybePermit = userSupplier.TryReserveSlot(ReserveCtxFromBridge(ctx));
            }
#pragma warning disable CA1031 // We are ok catching all exceptions here
            catch (Exception e)
            {
#pragma warning restore CA1031
                logger.LogError(e, "Error trying to reserve slot");
                return UIntPtr.Zero;
            }

            if (maybePermit == null)
            {
                return UIntPtr.Zero;
            }
            var usedPermitId = AddPermitToMap(maybePermit);
            return new(usedPermitId);
        }

        private unsafe void MarkUsed(Interop.TemporalCoreSlotMarkUsedCtx* ctx)
        {
            try
            {
                Temporalio.Worker.Tuning.SlotPermit permit;
                lock (permits)
                {
                    permit = permits[(*ctx).slot_permit.ToUInt32()];
                }
                userSupplier.MarkSlotUsed(MarkUsedCtxFromBridge(ctx, permit));
            }
#pragma warning disable CA1031 // We are ok catching all exceptions here
            catch (Exception e)
            {
#pragma warning restore CA1031
                logger.LogError(e, "Error marking slot used");
            }
        }

        private unsafe void Release(Interop.TemporalCoreSlotReleaseCtx* ctx)
        {
            var permitId = (*ctx).slot_permit.ToUInt32();
            Temporalio.Worker.Tuning.SlotPermit permit;
            lock (permits)
            {
                permit = permits[permitId];
            }
            try
            {
                userSupplier.ReleaseSlot(ReleaseCtxFromBridge(ctx, permit));
            }
#pragma warning disable CA1031 // We are ok catching all exceptions here
            catch (Exception e)
            {
#pragma warning restore CA1031
                logger.LogError(e, "Error releasing slot");
            }
            finally
            {
                lock (permits)
                {
                    permits.Remove(permitId);
                }
            }
        }

        private uint AddPermitToMap(Temporalio.Worker.Tuning.SlotPermit permit)
        {
            lock (permits)
            {
                var usedPermitId = permitId;
                permits.Add(permitId, permit);
                permitId += 1;
                return usedPermitId;
            }
        }

        private unsafe Temporalio.Worker.Tuning.SlotReserveContext ReserveCtxFromBridge(Interop.TemporalCoreSlotReserveCtx* ctx)
        {
            return new(
                SlotType: (*ctx).slot_type switch
                {
                    Interop.TemporalCoreSlotKindType.WorkflowSlotKindType => Temporalio.Worker.Tuning.SlotType.Workflow,
                    Interop.TemporalCoreSlotKindType.ActivitySlotKindType => Temporalio.Worker.Tuning.SlotType.Activity,
                    Interop.TemporalCoreSlotKindType.LocalActivitySlotKindType => Temporalio.Worker.Tuning.SlotType.LocalActivity,
                    _ => throw new System.ArgumentOutOfRangeException(nameof(ctx)),
                },
                TaskQueue: ByteArrayRef.ToUtf8((*ctx).task_queue),
                WorkerIdentity: ByteArrayRef.ToUtf8((*ctx).worker_identity),
                WorkerBuildId: ByteArrayRef.ToUtf8((*ctx).worker_build_id),
                IsSticky: (*ctx).is_sticky != 0);
        }

        private unsafe Temporalio.Worker.Tuning.SlotReleaseContext ReleaseCtxFromBridge(
            Interop.TemporalCoreSlotReleaseCtx* ctx,
            Temporalio.Worker.Tuning.SlotPermit permit)
        {
            return new(
                SlotInfo: (*ctx).slot_info is null ? null : SlotInfoFromBridge(*(*ctx).slot_info),
                Permit: permit);
        }

        private unsafe Temporalio.Worker.Tuning.SlotMarkUsedContext MarkUsedCtxFromBridge(
            Interop.TemporalCoreSlotMarkUsedCtx* ctx,
            Temporalio.Worker.Tuning.SlotPermit permit)
        {
            return new(
                SlotInfo: SlotInfoFromBridge((*ctx).slot_info),
                Permit: permit);
        }
    }
}
