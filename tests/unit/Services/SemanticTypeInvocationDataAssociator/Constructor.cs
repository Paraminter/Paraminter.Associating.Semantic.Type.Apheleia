namespace Paraminter.Associators.Semantic.Type.Apheleia;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void ReturnsAssociator()
    {
        var result = Target();

        Assert.NotNull(result);
    }

    private static SemanticTypeInvocationDataAssociator Target() => new();
}
