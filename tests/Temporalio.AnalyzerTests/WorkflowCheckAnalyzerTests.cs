namespace Temporalio.AnalyzerTests;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Temporalio.Analyzers;
using Temporalio.AnalyzerTests.SeparateProject;
using Xunit;

public class WorkflowCheckAnalyzerTests
{
    [Fact]
    public async Task SomeTest()
    {
        var otherProj = MetadataReference.CreateFromFile(typeof(DifferentCallsClass).Assembly.Location);
        var test = new CSharpAnalyzerTest<WorkflowCheckAnalyzer, DefaultVerifier>
        {
            TestCode = @"
                using System;
                using Temporalio.AnalyzerTests.SeparateProject;

                class Program
                {
                    static void Main()
                    {
                        DifferentCallsClass.DoSomethingStatic();
                    }
                }
            ",
            TestState =
            {
                AdditionalReferences = { otherProj },
            },
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60,
        };
        await test.RunAsync();
    }

    // TODO:
    // * Test static method, lambda, no-namespace, etc
    // * Overloads where only one is bad
}