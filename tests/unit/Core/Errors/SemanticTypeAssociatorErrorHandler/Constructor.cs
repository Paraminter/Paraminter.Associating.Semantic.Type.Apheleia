namespace Paraminter.Associating.Semantic.Type.Apheleia.Errors;

using Moq;

using Paraminter.Associating.Semantic.Type.Apheleia.Errors.Commands;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullDifferentNumberOfArgumentsAndParameters_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsHandler()
    {
        var result = Target(Mock.Of<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>>());

        Assert.NotNull(result);
    }

    private static SemanticTypeAssociatorErrorHandler Target(
        ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> differentNumberOfArgumentsAndParameters)
    {
        return new SemanticTypeAssociatorErrorHandler(differentNumberOfArgumentsAndParameters);
    }
}
