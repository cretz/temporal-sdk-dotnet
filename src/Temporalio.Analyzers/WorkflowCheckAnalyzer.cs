using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Temporalio.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WorkflowCheckAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new(
            id: "TMPRL1100",
            title: "Cannot make unsafe calls in Temporal workflow",
            messageFormat: "{0} is considered unsafe, reason: {1}",
            category: "Temporalio.Workflows",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Cannot make unsafe calls in Temporal workflow.",
            helpLinkUri: "https://example.com/todo-help link");
        // TODO(cretz): Hidden rule for recording non-deterministic-ness?

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(OnOperationAction, OperationKind.Invocation);
        }

        private void OnOperationAction(OperationAnalysisContext context)
        {
            var op = (IInvocationOperation)context.Operation;
            var asmName = "<unknown>";
            var list = new List<string> { };
            Console.WriteLine("Got operation: {0} - {1}.{2} in {3}", op, op.Instance, op.TargetMethod?.Name, asmName);

            if (op.TargetMethod?.ContainingAssembly is { } asmSym)
            {
                if (asmSym.Equals(context.Compilation.Assembly, SymbolEqualityComparer.Default))
                {
                    asmName = "<current>";
                }
                else
                {
                    var asmRef = context.Compilation.References.OfType<PortableExecutableReference>().
                        FirstOrDefault(r => r.GetMetadata() is AssemblyMetadata asmMeta &&
                            asmMeta.Id == asmSym.GetMetadata()?.Id);
                    asmName = asmRef?.FilePath ?? "<not-found>";
                    var className = op.Instance?.Type?.Name ?? $"{op.TargetMethod.ContainingNamespace?.ToDisplayString()}.{op.TargetMethod.ContainingType?.Name}";
                    Console.WriteLine("----\n{0}----", ExternalModule.Load(asmName, null, null).Dump());
                }
            }

            // TODO(cretz): Remove write-lines above and work with the call graph
        }
    }
}