namespace Temporalio.Analyzers
{
    // TODO(cretz): File and line number info? May not be important if we're not even
    //   differentiating between overloads.
    // TODO(cretz): "record struct" not supported in our version, but do we want readonly
    //   struct? We may come back later and make this mutable if we want to color it as
    //   non-deterministic.
    public record MethodRef(string Type, string Method);
}