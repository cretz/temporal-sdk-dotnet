// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: temporal/api/enums/v1/common.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Temporalio.Api.Enums.V1 {

  /// <summary>Holder for reflection information generated from temporal/api/enums/v1/common.proto</summary>
  public static partial class CommonReflection {

    #region Descriptor
    /// <summary>File descriptor for temporal/api/enums/v1/common.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommonReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiJ0ZW1wb3JhbC9hcGkvZW51bXMvdjEvY29tbW9uLnByb3RvEhV0ZW1wb3Jh",
            "bC5hcGkuZW51bXMudjEqXwoMRW5jb2RpbmdUeXBlEh0KGUVOQ09ESU5HX1RZ",
            "UEVfVU5TUEVDSUZJRUQQABIYChRFTkNPRElOR19UWVBFX1BST1RPMxABEhYK",
            "EkVOQ09ESU5HX1RZUEVfSlNPThACKpECChBJbmRleGVkVmFsdWVUeXBlEiIK",
            "HklOREVYRURfVkFMVUVfVFlQRV9VTlNQRUNJRklFRBAAEhsKF0lOREVYRURf",
            "VkFMVUVfVFlQRV9URVhUEAESHgoaSU5ERVhFRF9WQUxVRV9UWVBFX0tFWVdP",
            "UkQQAhIaChZJTkRFWEVEX1ZBTFVFX1RZUEVfSU5UEAMSHQoZSU5ERVhFRF9W",
            "QUxVRV9UWVBFX0RPVUJMRRAEEhsKF0lOREVYRURfVkFMVUVfVFlQRV9CT09M",
            "EAUSHwobSU5ERVhFRF9WQUxVRV9UWVBFX0RBVEVUSU1FEAYSIwofSU5ERVhF",
            "RF9WQUxVRV9UWVBFX0tFWVdPUkRfTElTVBAHKl4KCFNldmVyaXR5EhgKFFNF",
            "VkVSSVRZX1VOU1BFQ0lGSUVEEAASEQoNU0VWRVJJVFlfSElHSBABEhMKD1NF",
            "VkVSSVRZX01FRElVTRACEhAKDFNFVkVSSVRZX0xPVxADKsIBCg1DYWxsYmFj",
            "a1N0YXRlEh4KGkNBTExCQUNLX1NUQVRFX1VOU1BFQ0lGSUVEEAASGgoWQ0FM",
            "TEJBQ0tfU1RBVEVfU1RBTkRCWRABEhwKGENBTExCQUNLX1NUQVRFX1NDSEVE",
            "VUxFRBACEh4KGkNBTExCQUNLX1NUQVRFX0JBQ0tJTkdfT0ZGEAMSGQoVQ0FM",
            "TEJBQ0tfU1RBVEVfRkFJTEVEEAQSHAoYQ0FMTEJBQ0tfU1RBVEVfU1VDQ0VF",
            "REVEEAUq0gEKGlBlbmRpbmdOZXh1c09wZXJhdGlvblN0YXRlEi0KKVBFTkRJ",
            "TkdfTkVYVVNfT1BFUkFUSU9OX1NUQVRFX1VOU1BFQ0lGSUVEEAASKwonUEVO",
            "RElOR19ORVhVU19PUEVSQVRJT05fU1RBVEVfU0NIRURVTEVEEAESLQopUEVO",
            "RElOR19ORVhVU19PUEVSQVRJT05fU1RBVEVfQkFDS0lOR19PRkYQAhIpCiVQ",
            "RU5ESU5HX05FWFVTX09QRVJBVElPTl9TVEFURV9TVEFSVEVEEAMqzgIKH05l",
            "eHVzT3BlcmF0aW9uQ2FuY2VsbGF0aW9uU3RhdGUSMgouTkVYVVNfT1BFUkFU",
            "SU9OX0NBTkNFTExBVElPTl9TVEFURV9VTlNQRUNJRklFRBAAEjAKLE5FWFVT",
            "X09QRVJBVElPTl9DQU5DRUxMQVRJT05fU1RBVEVfU0NIRURVTEVEEAESMgou",
            "TkVYVVNfT1BFUkFUSU9OX0NBTkNFTExBVElPTl9TVEFURV9CQUNLSU5HX09G",
            "RhACEjAKLE5FWFVTX09QRVJBVElPTl9DQU5DRUxMQVRJT05fU1RBVEVfU1VD",
            "Q0VFREVEEAMSLQopTkVYVVNfT1BFUkFUSU9OX0NBTkNFTExBVElPTl9TVEFU",
            "RV9GQUlMRUQQBBIwCixORVhVU19PUEVSQVRJT05fQ0FOQ0VMTEFUSU9OX1NU",
            "QVRFX1RJTUVEX09VVBAFQoMBChhpby50ZW1wb3JhbC5hcGkuZW51bXMudjFC",
            "C0NvbW1vblByb3RvUAFaIWdvLnRlbXBvcmFsLmlvL2FwaS9lbnVtcy92MTtl",
            "bnVtc6oCF1RlbXBvcmFsaW8uQXBpLkVudW1zLlYx6gIaVGVtcG9yYWxpbzo6",
            "QXBpOjpFbnVtczo6VjFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Temporalio.Api.Enums.V1.EncodingType), typeof(global::Temporalio.Api.Enums.V1.IndexedValueType), typeof(global::Temporalio.Api.Enums.V1.Severity), typeof(global::Temporalio.Api.Enums.V1.CallbackState), typeof(global::Temporalio.Api.Enums.V1.PendingNexusOperationState), typeof(global::Temporalio.Api.Enums.V1.NexusOperationCancellationState), }, null, null));
    }
    #endregion

  }
  #region Enums
  public enum EncodingType {
    [pbr::OriginalName("ENCODING_TYPE_UNSPECIFIED")] Unspecified = 0,
    [pbr::OriginalName("ENCODING_TYPE_PROTO3")] Proto3 = 1,
    [pbr::OriginalName("ENCODING_TYPE_JSON")] Json = 2,
  }

  public enum IndexedValueType {
    [pbr::OriginalName("INDEXED_VALUE_TYPE_UNSPECIFIED")] Unspecified = 0,
    [pbr::OriginalName("INDEXED_VALUE_TYPE_TEXT")] Text = 1,
    [pbr::OriginalName("INDEXED_VALUE_TYPE_KEYWORD")] Keyword = 2,
    [pbr::OriginalName("INDEXED_VALUE_TYPE_INT")] Int = 3,
    [pbr::OriginalName("INDEXED_VALUE_TYPE_DOUBLE")] Double = 4,
    [pbr::OriginalName("INDEXED_VALUE_TYPE_BOOL")] Bool = 5,
    [pbr::OriginalName("INDEXED_VALUE_TYPE_DATETIME")] Datetime = 6,
    [pbr::OriginalName("INDEXED_VALUE_TYPE_KEYWORD_LIST")] KeywordList = 7,
  }

  public enum Severity {
    [pbr::OriginalName("SEVERITY_UNSPECIFIED")] Unspecified = 0,
    [pbr::OriginalName("SEVERITY_HIGH")] High = 1,
    [pbr::OriginalName("SEVERITY_MEDIUM")] Medium = 2,
    [pbr::OriginalName("SEVERITY_LOW")] Low = 3,
  }

  /// <summary>
  /// State of a callback.
  /// </summary>
  public enum CallbackState {
    /// <summary>
    /// Default value, unspecified state.
    /// </summary>
    [pbr::OriginalName("CALLBACK_STATE_UNSPECIFIED")] Unspecified = 0,
    /// <summary>
    /// Callback is standing by, waiting to be triggered.
    /// </summary>
    [pbr::OriginalName("CALLBACK_STATE_STANDBY")] Standby = 1,
    /// <summary>
    /// Callback is in the queue waiting to be executed or is currently executing.
    /// </summary>
    [pbr::OriginalName("CALLBACK_STATE_SCHEDULED")] Scheduled = 2,
    /// <summary>
    /// Callback has failed with a retryable error and is backing off before the next attempt.
    /// </summary>
    [pbr::OriginalName("CALLBACK_STATE_BACKING_OFF")] BackingOff = 3,
    /// <summary>
    /// Callback has failed.
    /// </summary>
    [pbr::OriginalName("CALLBACK_STATE_FAILED")] Failed = 4,
    /// <summary>
    /// Callback has succeeded.
    /// </summary>
    [pbr::OriginalName("CALLBACK_STATE_SUCCEEDED")] Succeeded = 5,
  }

  /// <summary>
  /// State of a pending Nexus operation.
  /// </summary>
  public enum PendingNexusOperationState {
    /// <summary>
    /// Default value, unspecified state.
    /// </summary>
    [pbr::OriginalName("PENDING_NEXUS_OPERATION_STATE_UNSPECIFIED")] Unspecified = 0,
    /// <summary>
    /// Operation is in the queue waiting to be executed or is currently executing.
    /// </summary>
    [pbr::OriginalName("PENDING_NEXUS_OPERATION_STATE_SCHEDULED")] Scheduled = 1,
    /// <summary>
    /// Operation has failed with a retryable error and is backing off before the next attempt.
    /// </summary>
    [pbr::OriginalName("PENDING_NEXUS_OPERATION_STATE_BACKING_OFF")] BackingOff = 2,
    /// <summary>
    /// Operation was started and will complete asynchronously.
    /// </summary>
    [pbr::OriginalName("PENDING_NEXUS_OPERATION_STATE_STARTED")] Started = 3,
  }

  /// <summary>
  /// State of a Nexus operation cancellation.
  /// </summary>
  public enum NexusOperationCancellationState {
    /// <summary>
    /// Default value, unspecified state.
    /// </summary>
    [pbr::OriginalName("NEXUS_OPERATION_CANCELLATION_STATE_UNSPECIFIED")] Unspecified = 0,
    /// <summary>
    /// Cancellation request is in the queue waiting to be executed or is currently executing.
    /// </summary>
    [pbr::OriginalName("NEXUS_OPERATION_CANCELLATION_STATE_SCHEDULED")] Scheduled = 1,
    /// <summary>
    /// Cancellation request has failed with a retryable error and is backing off before the next attempt.
    /// </summary>
    [pbr::OriginalName("NEXUS_OPERATION_CANCELLATION_STATE_BACKING_OFF")] BackingOff = 2,
    /// <summary>
    /// Cancellation request succeeded.
    /// </summary>
    [pbr::OriginalName("NEXUS_OPERATION_CANCELLATION_STATE_SUCCEEDED")] Succeeded = 3,
    /// <summary>
    /// Cancellation request failed with a non-retryable error.
    /// </summary>
    [pbr::OriginalName("NEXUS_OPERATION_CANCELLATION_STATE_FAILED")] Failed = 4,
    /// <summary>
    /// The associated operation timed out - exceeded the user supplied schedule-to-close timeout.
    /// </summary>
    [pbr::OriginalName("NEXUS_OPERATION_CANCELLATION_STATE_TIMED_OUT")] TimedOut = 5,
  }

  #endregion

}

#endregion Designer generated code
