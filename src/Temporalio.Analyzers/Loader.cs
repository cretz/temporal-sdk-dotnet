using System;
using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Temporalio.Analyzers
{
    internal class Loader
    {
        private readonly Compilation compilation;
        private readonly ExternalModule.ICache externalModuleCache;
        private readonly InvalidCalls invalidCalls;
        private readonly ConcurrentDictionary<string, ExternalModule> externalModules = new();

        public Loader(
            CompilationStartAnalysisContext context,
            ExternalModule.ICache externalModuleCache,
            InvalidCalls invalidCalls)
        {
            compilation = context.Compilation;
            this.externalModuleCache = externalModuleCache;
            this.invalidCalls = invalidCalls;

            // TODO: Register invocation operations, and maybe method body end operations to do
            //  pending callbacks

        }

        private void OnInvocationOperation(OperationAnalysisContext context)
        {
            var op = (IInvocationOperation)context.Operation;
            if (op.TargetMethod is not { } method ||
                (op.ConstrainedToType ?? op.Instance?.Type ?? op.TargetMethod?.ContainingType) is not { } type)
            {
                return;
            }

            // We must check if the invocation is invalid. The invocation is invalid if one of the
            // two are true:
            // * Configured invalid set in the hierarchy (most-specific wins)
            // * Actual resolved implementation of the method has invalid calls itself
            // The first check can be done without processing invalidity of the method, but the
            // second cannot. And when processing invalidity for the second, it's possible we have
            // not traversed that method yet if it's in this same assembly, therefore we have to use
            // "pending" markers to come back and tell us if it is invalid.

            // First check whether the invocation is configured invalid
            var configuredInvalidResolution = new ConfiguredInvalidResolution();
            if (IsConfiguredInvalid(type, method, ref configuredInvalidResolution))
            {
                // TODO
                return;
            }

            // Now we process validity for the resolved method
            // TODO

            // // See if this is local or external
            // var asmSym = op.TargetMethod?.ContainingAssembly;
            // if (asmSym.Equals(compilation.Assembly, SymbolEqualityComparer.Default))
            // {
            //     // This is local, under method-specific lock, try to eagerly find non-deterministic
            //     // or register as pending
            //     // TODO(cretz): This
            // }
            // else
            // {
            //     // This is external, load
            // }
        }

        private bool IsConfiguredInvalid(ITypeSymbol type, IMethodSymbol method, ref ConfiguredInvalidResolution resolution)
        {
            // TODO
            throw new NotImplementedException();
        }

        private struct ConfiguredInvalidResolution
        {
            // TODO
        }
    }
}