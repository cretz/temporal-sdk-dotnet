namespace Temporalio.AnalyzerTests;

using System;
using System.Reflection.Metadata;
using Temporalio.Analyzers;
using Temporalio.AnalyzerTests.SeparateProject;
using Xunit;

public class ExternalModuleTests
{
    [Fact]
    public void Load_MethodRefs_AreAccurate()
    {
        var reporter = new DebugReporter();
        var mod = ExternalModule.Load(typeof(DifferentCallsClass).Assembly.Location, null, reporter);

        void AssertMethodRef(
            string calleeType,
            string calleeMethod,
            string targetType,
            string targetMethod,
            ILOpCode opCode,
            Type handleType)
        {
            var methRef = mod.RefGraph[calleeType][calleeMethod]!.First(
                c => c.Type == targetType && c.Method == targetMethod);
            Assert.Contains(
                reporter.Refs,
                c => c.Ref == methRef && c.OpCode == opCode && c.OrigHandleType == handleType);
        }

        const string ns = "Temporalio.AnalyzerTests.SeparateProject";
        const string cls = $"{ns}.DifferentCallsClass";
        const string meth = "DoSomethingStatic";
        var methDef = typeof(MethodDefinitionHandle);
        var memRef = typeof(MemberReferenceHandle);

        AssertMethodRef(cls, meth, cls, meth, ILOpCode.Call, methDef);
        AssertMethodRef(cls, meth, cls, ".ctor", ILOpCode.Newobj, methDef);
        AssertMethodRef(cls, meth, cls, "DoSomethingInstance", ILOpCode.Callvirt, methDef);
        AssertMethodRef(cls, meth, cls, "DoSomethingStatic2", ILOpCode.Ldftn, methDef);
        AssertMethodRef(cls, meth, "System.Action", ".ctor", ILOpCode.Newobj, memRef);
        AssertMethodRef(cls, meth, "System.Action", "Invoke", ILOpCode.Callvirt, memRef);
        AssertMethodRef(cls, meth, cls, "DoSomethingInstance2", ILOpCode.Ldvirtftn, methDef);
    }

    public class DebugReporter : ExternalModule.IDebugReporter
    {
        public List<(MethodRef Ref, ILOpCode OpCode, Type OrigHandleType)> Refs { get; } = new();

        public void OnMethodRef(MethodRef methRef, ILOpCode opCode, Type origHandleType) =>
            Refs.Add(new(methRef, opCode, origHandleType));
    }
}