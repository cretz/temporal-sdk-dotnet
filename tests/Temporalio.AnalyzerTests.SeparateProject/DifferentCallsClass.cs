namespace Temporalio.AnalyzerTests.SeparateProject;

public class DifferentCallsClass
{
    public static void DoSomethingStatic(bool recurse)
    {
        if (recurse)
        {
            // Call + MethodDefinitionHandle
            DoSomethingStatic(false);
        }
        // Newobj + MethodDefinitionHandle
        var temp = new DifferentCallsClass();
        // Callvirt + MethodDefinitionHandle
        temp.DoSomethingInstance();
        // Ldftn + MethodDefinitionHandle
        // Newobj + MemberReferenceHandle
        var toCallStatic = DoSomethingStatic2;
        // Callvirt + MemberReferenceHandle
        toCallStatic.Invoke();
        // Ldvirtftn + MethodDefinitionHandle
        var toCallInstance = temp.DoSomethingInstance2;
        toCallInstance.Invoke();
    }

    public static void DoSomethingStatic2()
    {
        // Do nothing
    }

    public virtual void DoSomethingInstance()
    {
        // Do nothing
    }

    internal virtual void DoSomethingInstance2()
    {
        // Do nothing
    }
}