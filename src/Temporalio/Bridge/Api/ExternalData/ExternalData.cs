// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: temporal/sdk/core/external_data/external_data.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Temporalio.Bridge.Api.ExternalData {

  /// <summary>Holder for reflection information generated from temporal/sdk/core/external_data/external_data.proto</summary>
  internal static partial class ExternalDataReflection {

    #region Descriptor
    /// <summary>File descriptor for temporal/sdk/core/external_data/external_data.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ExternalDataReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjN0ZW1wb3JhbC9zZGsvY29yZS9leHRlcm5hbF9kYXRhL2V4dGVybmFsX2Rh",
            "dGEucHJvdG8SFWNvcmVzZGsuZXh0ZXJuYWxfZGF0YRoeZ29vZ2xlL3Byb3Rv",
            "YnVmL2R1cmF0aW9uLnByb3RvGh9nb29nbGUvcHJvdG9idWYvdGltZXN0YW1w",
            "LnByb3RvIv4BChdMb2NhbEFjdGl2aXR5TWFya2VyRGF0YRILCgNzZXEYASAB",
            "KA0SDwoHYXR0ZW1wdBgCIAEoDRITCgthY3Rpdml0eV9pZBgDIAEoCRIVCg1h",
            "Y3Rpdml0eV90eXBlGAQgASgJEjEKDWNvbXBsZXRlX3RpbWUYBSABKAsyGi5n",
            "b29nbGUucHJvdG9idWYuVGltZXN0YW1wEioKB2JhY2tvZmYYBiABKAsyGS5n",
            "b29nbGUucHJvdG9idWYuRHVyYXRpb24SOgoWb3JpZ2luYWxfc2NoZWR1bGVf",
            "dGltZRgHIAEoCzIaLmdvb2dsZS5wcm90b2J1Zi5UaW1lc3RhbXAiMwoRUGF0",
            "Y2hlZE1hcmtlckRhdGESCgoCaWQYASABKAkSEgoKZGVwcmVjYXRlZBgCIAEo",
            "CEIy6gIvVGVtcG9yYWxpbzo6SW50ZXJuYWw6OkJyaWRnZTo6QXBpOjpFeHRl",
            "cm5hbERhdGFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.DurationReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Temporalio.Bridge.Api.ExternalData.LocalActivityMarkerData), global::Temporalio.Bridge.Api.ExternalData.LocalActivityMarkerData.Parser, new[]{ "Seq", "Attempt", "ActivityId", "ActivityType", "CompleteTime", "Backoff", "OriginalScheduleTime" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Temporalio.Bridge.Api.ExternalData.PatchedMarkerData), global::Temporalio.Bridge.Api.ExternalData.PatchedMarkerData.Parser, new[]{ "Id", "Deprecated" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  internal sealed partial class LocalActivityMarkerData : pb::IMessage<LocalActivityMarkerData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<LocalActivityMarkerData> _parser = new pb::MessageParser<LocalActivityMarkerData>(() => new LocalActivityMarkerData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<LocalActivityMarkerData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Temporalio.Bridge.Api.ExternalData.ExternalDataReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LocalActivityMarkerData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LocalActivityMarkerData(LocalActivityMarkerData other) : this() {
      seq_ = other.seq_;
      attempt_ = other.attempt_;
      activityId_ = other.activityId_;
      activityType_ = other.activityType_;
      completeTime_ = other.completeTime_ != null ? other.completeTime_.Clone() : null;
      backoff_ = other.backoff_ != null ? other.backoff_.Clone() : null;
      originalScheduleTime_ = other.originalScheduleTime_ != null ? other.originalScheduleTime_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LocalActivityMarkerData Clone() {
      return new LocalActivityMarkerData(this);
    }

    /// <summary>Field number for the "seq" field.</summary>
    public const int SeqFieldNumber = 1;
    private uint seq_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Seq {
      get { return seq_; }
      set {
        seq_ = value;
      }
    }

    /// <summary>Field number for the "attempt" field.</summary>
    public const int AttemptFieldNumber = 2;
    private uint attempt_;
    /// <summary>
    /// The number of attempts at execution before we recorded this result. Typically starts at 1,
    /// but it is possible to start at a higher number when backing off using a timer.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Attempt {
      get { return attempt_; }
      set {
        attempt_ = value;
      }
    }

    /// <summary>Field number for the "activity_id" field.</summary>
    public const int ActivityIdFieldNumber = 3;
    private string activityId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string ActivityId {
      get { return activityId_; }
      set {
        activityId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "activity_type" field.</summary>
    public const int ActivityTypeFieldNumber = 4;
    private string activityType_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string ActivityType {
      get { return activityType_; }
      set {
        activityType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "complete_time" field.</summary>
    public const int CompleteTimeFieldNumber = 5;
    private global::Google.Protobuf.WellKnownTypes.Timestamp completeTime_;
    /// <summary>
    /// You can think of this as "perceived completion time". It is the time the local activity thought
    /// it was when it completed. Which could be different from wall-clock time because of workflow
    /// replay. It's the WFT start time + the LA's runtime
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Timestamp CompleteTime {
      get { return completeTime_; }
      set {
        completeTime_ = value;
      }
    }

    /// <summary>Field number for the "backoff" field.</summary>
    public const int BackoffFieldNumber = 6;
    private global::Google.Protobuf.WellKnownTypes.Duration backoff_;
    /// <summary>
    /// If set, this local activity conceptually is retrying after the specified backoff.
    /// Implementation wise, they are really two different LA machines, but with the same type &amp; input.
    /// The retry starts with an attempt number > 1.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Duration Backoff {
      get { return backoff_; }
      set {
        backoff_ = value;
      }
    }

    /// <summary>Field number for the "original_schedule_time" field.</summary>
    public const int OriginalScheduleTimeFieldNumber = 7;
    private global::Google.Protobuf.WellKnownTypes.Timestamp originalScheduleTime_;
    /// <summary>
    /// The time the LA was originally scheduled (wall clock time). This is used to track
    /// schedule-to-close timeouts when timer-based backoffs are used
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Timestamp OriginalScheduleTime {
      get { return originalScheduleTime_; }
      set {
        originalScheduleTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as LocalActivityMarkerData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(LocalActivityMarkerData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Seq != other.Seq) return false;
      if (Attempt != other.Attempt) return false;
      if (ActivityId != other.ActivityId) return false;
      if (ActivityType != other.ActivityType) return false;
      if (!object.Equals(CompleteTime, other.CompleteTime)) return false;
      if (!object.Equals(Backoff, other.Backoff)) return false;
      if (!object.Equals(OriginalScheduleTime, other.OriginalScheduleTime)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Seq != 0) hash ^= Seq.GetHashCode();
      if (Attempt != 0) hash ^= Attempt.GetHashCode();
      if (ActivityId.Length != 0) hash ^= ActivityId.GetHashCode();
      if (ActivityType.Length != 0) hash ^= ActivityType.GetHashCode();
      if (completeTime_ != null) hash ^= CompleteTime.GetHashCode();
      if (backoff_ != null) hash ^= Backoff.GetHashCode();
      if (originalScheduleTime_ != null) hash ^= OriginalScheduleTime.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Seq != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(Seq);
      }
      if (Attempt != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Attempt);
      }
      if (ActivityId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ActivityId);
      }
      if (ActivityType.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ActivityType);
      }
      if (completeTime_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(CompleteTime);
      }
      if (backoff_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(Backoff);
      }
      if (originalScheduleTime_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(OriginalScheduleTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Seq != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(Seq);
      }
      if (Attempt != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Attempt);
      }
      if (ActivityId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ActivityId);
      }
      if (ActivityType.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ActivityType);
      }
      if (completeTime_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(CompleteTime);
      }
      if (backoff_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(Backoff);
      }
      if (originalScheduleTime_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(OriginalScheduleTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Seq != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Seq);
      }
      if (Attempt != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Attempt);
      }
      if (ActivityId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ActivityId);
      }
      if (ActivityType.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ActivityType);
      }
      if (completeTime_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CompleteTime);
      }
      if (backoff_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Backoff);
      }
      if (originalScheduleTime_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(OriginalScheduleTime);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(LocalActivityMarkerData other) {
      if (other == null) {
        return;
      }
      if (other.Seq != 0) {
        Seq = other.Seq;
      }
      if (other.Attempt != 0) {
        Attempt = other.Attempt;
      }
      if (other.ActivityId.Length != 0) {
        ActivityId = other.ActivityId;
      }
      if (other.ActivityType.Length != 0) {
        ActivityType = other.ActivityType;
      }
      if (other.completeTime_ != null) {
        if (completeTime_ == null) {
          CompleteTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        CompleteTime.MergeFrom(other.CompleteTime);
      }
      if (other.backoff_ != null) {
        if (backoff_ == null) {
          Backoff = new global::Google.Protobuf.WellKnownTypes.Duration();
        }
        Backoff.MergeFrom(other.Backoff);
      }
      if (other.originalScheduleTime_ != null) {
        if (originalScheduleTime_ == null) {
          OriginalScheduleTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        OriginalScheduleTime.MergeFrom(other.OriginalScheduleTime);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Seq = input.ReadUInt32();
            break;
          }
          case 16: {
            Attempt = input.ReadUInt32();
            break;
          }
          case 26: {
            ActivityId = input.ReadString();
            break;
          }
          case 34: {
            ActivityType = input.ReadString();
            break;
          }
          case 42: {
            if (completeTime_ == null) {
              CompleteTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(CompleteTime);
            break;
          }
          case 50: {
            if (backoff_ == null) {
              Backoff = new global::Google.Protobuf.WellKnownTypes.Duration();
            }
            input.ReadMessage(Backoff);
            break;
          }
          case 58: {
            if (originalScheduleTime_ == null) {
              OriginalScheduleTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(OriginalScheduleTime);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Seq = input.ReadUInt32();
            break;
          }
          case 16: {
            Attempt = input.ReadUInt32();
            break;
          }
          case 26: {
            ActivityId = input.ReadString();
            break;
          }
          case 34: {
            ActivityType = input.ReadString();
            break;
          }
          case 42: {
            if (completeTime_ == null) {
              CompleteTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(CompleteTime);
            break;
          }
          case 50: {
            if (backoff_ == null) {
              Backoff = new global::Google.Protobuf.WellKnownTypes.Duration();
            }
            input.ReadMessage(Backoff);
            break;
          }
          case 58: {
            if (originalScheduleTime_ == null) {
              OriginalScheduleTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(OriginalScheduleTime);
            break;
          }
        }
      }
    }
    #endif

  }

  internal sealed partial class PatchedMarkerData : pb::IMessage<PatchedMarkerData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<PatchedMarkerData> _parser = new pb::MessageParser<PatchedMarkerData>(() => new PatchedMarkerData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<PatchedMarkerData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Temporalio.Bridge.Api.ExternalData.ExternalDataReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PatchedMarkerData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PatchedMarkerData(PatchedMarkerData other) : this() {
      id_ = other.id_;
      deprecated_ = other.deprecated_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PatchedMarkerData Clone() {
      return new PatchedMarkerData(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private string id_ = "";
    /// <summary>
    /// The patch id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "deprecated" field.</summary>
    public const int DeprecatedFieldNumber = 2;
    private bool deprecated_;
    /// <summary>
    /// Whether or not the patch is marked deprecated.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Deprecated {
      get { return deprecated_; }
      set {
        deprecated_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as PatchedMarkerData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(PatchedMarkerData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Deprecated != other.Deprecated) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (Deprecated != false) hash ^= Deprecated.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Id);
      }
      if (Deprecated != false) {
        output.WriteRawTag(16);
        output.WriteBool(Deprecated);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Id);
      }
      if (Deprecated != false) {
        output.WriteRawTag(16);
        output.WriteBool(Deprecated);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (Deprecated != false) {
        size += 1 + 1;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(PatchedMarkerData other) {
      if (other == null) {
        return;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      if (other.Deprecated != false) {
        Deprecated = other.Deprecated;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 16: {
            Deprecated = input.ReadBool();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 16: {
            Deprecated = input.ReadBool();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
