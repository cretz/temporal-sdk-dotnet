using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Temporalio.Analyzers
{
    public record ExternalModule(
        string Name,
        Guid VersionId,
        string DllPath,
        Dictionary<string, Dictionary<string, List<MethodRef>?>> RefGraph)
    {
        public interface IDebugReporter
        {
            void OnMethodRef(MethodRef methRef, ILOpCode opCode, Type origHandleType);
        }

        public interface ICache
        {
            // TODO(cretz): Document the Func must be run within this call
            ExternalModule GetOrAdd(string dllPath, string name, Guid versionId, Func<ExternalModule> create);
        }

        public static ExternalModule Load(string dllPath, ICache? cache, IDebugReporter? debugReporter)
        {
            using var stream = File.OpenRead(dllPath);
            using var peReader = new PEReader(stream);
            var reader = peReader.GetMetadataReader();

            var name = reader.GetString(reader.GetModuleDefinition().Name);
            var versionId = reader.GetGuid(reader.GetModuleDefinition().Mvid);

            if (cache is { } presentCache)
            {
                return presentCache.GetOrAdd(dllPath, name, versionId, () =>
                    Load(dllPath, debugReporter, name, versionId, peReader, reader));
            }
            return Load(dllPath, debugReporter, name, versionId, peReader, reader);
        }

        private static ExternalModule Load(
            string dllPath,
            IDebugReporter? debugReporter,
            string name,
            Guid versionId,
            PEReader peReader,
            MetadataReader reader)
        {
            var refGraph = new Dictionary<string, Dictionary<string, List<MethodRef>?>>();
            foreach (var typeDefHandle in reader.TypeDefinitions)
            {
                var typeDef = reader.GetTypeDefinition(typeDefHandle);
                Dictionary<string, List<MethodRef>?> methods = new();
                foreach (var methodDefHandle in typeDef.GetMethods())
                {
                    var methodDef = reader.GetMethodDefinition(methodDefHandle);
                    var methodBody = peReader.GetMethodBody(methodDef.RelativeVirtualAddress);
                    if (methodBody != null && GetRefMethods(
                        reader, methodBody.GetILReader(), debugReporter) is { } refs)
                    {
                        var methodName = reader.GetString(methodDef.Name);
                        if (methods.TryGetValue(methodName, out var existingRefs) && existingRefs != null)
                        {
                            existingRefs.AddRange(refs);
                        }
                        else
                        {
                            methods[methodName] = refs;
                        }
                    }
                }
                var ns = reader.GetString(typeDef.Namespace);
                var typeName = reader.GetString(typeDef.Name);
                refGraph[$"{ns}.{typeName}"] = methods;
            }
            return new(name, versionId, dllPath, refGraph);
        }

        private static List<MethodRef>? GetRefMethods(
            MetadataReader reader, BlobReader ilReader, IDebugReporter? debugReporter)
        {
            List<MethodRef>? refs = null;
            while (ilReader.RemainingBytes > 0)
            {
                var opCode = ilReader.ReadOpCode();
                switch (opCode)
                {
                    case ILOpCode.Call:
                    case ILOpCode.Callvirt:
                    case ILOpCode.Newobj:
                    case ILOpCode.Ldftn:
                    case ILOpCode.Ldvirtftn:
                    case ILOpCode.Calli:
                        int token = ilReader.ReadInt32();
                        if (GetRefFromHandle(
                            reader, MetadataTokens.EntityHandle(token), opCode, debugReporter) is { } methRef)
                        {
                            refs ??= new();
                            refs.Add(methRef);
                        }
                        break;
                    default:
                        ilReader.SkipOperand(opCode);
                        break;
                }
            }
            return refs;
        }

        private static MethodRef? GetRefFromHandle(
            MetadataReader reader,
            EntityHandle handle,
            ILOpCode opCode,
            IDebugReporter? debugReporter,
            Type? origHandleType = null)
        {
            switch (handle.Kind)
            {
                case HandleKind.MethodDefinition:
                    var methodDef = reader.GetMethodDefinition((MethodDefinitionHandle)handle);
                    var typeDef = reader.GetTypeDefinition(methodDef.GetDeclaringType());
                    origHandleType ??= typeof(MethodDefinitionHandle);
                    var typeName1 = $"{reader.GetString(typeDef.Namespace)}.{reader.GetString(typeDef.Name)}";
                    var ref1 = new MethodRef(typeName1, reader.GetString(methodDef.Name));
                    debugReporter?.OnMethodRef(ref1, opCode, origHandleType ?? typeof(MethodDefinitionHandle));
                    return ref1;
                case HandleKind.MemberReference:
                    var memberRef = reader.GetMemberReference((MemberReferenceHandle)handle);
                    if (GetTypeNameFromHandle(reader, memberRef.Parent) is not { } typeName2)
                    {
                        return null;
                    }
                    var ref2 = new MethodRef(typeName2, reader.GetString(memberRef.Name));
                    debugReporter?.OnMethodRef(ref2, opCode, origHandleType ?? typeof(MemberReferenceHandle));
                    return ref2;
                case HandleKind.MethodSpecification:
                    var methodSpec = reader.GetMethodSpecification((MethodSpecificationHandle)handle);
                    origHandleType ??= typeof(MethodSpecificationHandle);
                    return GetRefFromHandle(
                        reader, methodSpec.Method, opCode, debugReporter, origHandleType ?? typeof(MethodSpecificationHandle));
                default:
                    return null;
            }
        }

        private static string? GetTypeNameFromHandle(MetadataReader reader, EntityHandle handle)
        {
            switch (handle.Kind)
            {
                case HandleKind.TypeDefinition:
                    var typeDef = reader.GetTypeDefinition((TypeDefinitionHandle)handle);
                    return $"{reader.GetString(typeDef.Namespace)}.{reader.GetString(typeDef.Name)}";
                case HandleKind.TypeReference:
                    var typeRef = reader.GetTypeReference((TypeReferenceHandle)handle);
                    return $"{reader.GetString(typeRef.Namespace)}.{reader.GetString(typeRef.Name)}";
                default:
                    return null;
            }
        }

        public string Dump()
        {
            var dump = $"Module {Name} at {DllPath} (version: {VersionId})\n";
            foreach (var typeAndMethods in RefGraph)
            {
                dump += $"  Type: {typeAndMethods.Key}\n";
                foreach (var methodAndRefs in typeAndMethods.Value)
                {
                    dump += $"    Method: {methodAndRefs.Key}\n";
                    foreach (var methRef in methodAndRefs.Value)
                    {
                        dump += $"      Ref: {methRef}\n";
                    }
                }
            }
            return dump;
        }
    }
}