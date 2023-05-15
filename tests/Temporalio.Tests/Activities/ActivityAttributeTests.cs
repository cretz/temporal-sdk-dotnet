namespace Temporalio.Tests.Activities;

using System.Threading.Tasks;
using Temporalio.Activities;
using Xunit;

public class ActivityAttributeTests
{
    [Fact]
    public void Create_MissingAttribute_Throws()
    {
        var exc = Assert.ThrowsAny<Exception>(() => ActivityDefinition.Create(BadAct1));
        Assert.Contains("missing Activity attribute", exc.Message);
    }

    [Fact]
    public void Create_RefParameter_Throws()
    {
        var exc = Assert.ThrowsAny<Exception>(() => ActivityDefinition.Create(BadAct2));
        Assert.Contains("has disallowed ref/out parameter", exc.Message);
    }

    [Fact]
    public void Create_DefaultNameWithAsync_RemovesAsyncSuffix()
    {
        Assert.Equal("GoodAct1", ActivityDefinition.Create(GoodAct1Async).Name);
    }

    [Fact]
    public void Create_DefaultNameOnGeneric_ProperlyNamed()
    {
        [Activity]
        static Task<T> DoThingAsync<T>(T arg) => throw new NotImplementedException();
        Assert.Equal("DoThing", ActivityDefinition.Create(DoThingAsync<string>).Name);
    }

    [Fact]
    public void Create_LocalFunctionDefaultNames_AreAccurate()
    {
        [Activity]
        static string StaticDoThing() => string.Empty;
        Assert.Equal("StaticDoThing", ActivityDefinition.Create(StaticDoThing).Name);

        var val = "some val";
        [Activity]
        string DoThing() => val!;
        Assert.Equal("DoThing", ActivityDefinition.Create(DoThing).Name);
    }

    [Fact]
    public void Create_Lambda_Succeeds()
    {
        var def = ActivityDefinition.Create([Activity("MyActivity")] () => string.Empty);
        Assert.Equal("MyActivity", def.Name);
    }

    [Fact]
    public void Create_DefaultNameOnLambda_Throws()
    {
        var exc = Assert.ThrowsAny<Exception>(() =>
            ActivityDefinition.Create([Activity] () => string.Empty));
        Assert.Contains("appears to be a lambda", exc.Message);
    }

    [Fact]
    public void Create_Delegate_CanInvoke()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Create_DelegateWithDefaultParameter_CanInvoke()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Create_AsyncDelegate_CanInvoke()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void CreateAll_ClassWithoutActivities_Throws()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void CreateAll_ClassOfActivities_CanInvoke()
    {
        // TODO(cretz): Add a method with a default
        throw new NotImplementedException();
    }

    protected static void BadAct1()
    {
    }

    [Activity]
    protected static void BadAct2(ref string foo)
    {
    }

    [Activity]
    protected static Task GoodAct1Async() => Task.CompletedTask;
}