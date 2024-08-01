namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Type.Commands;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullRecorder_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IRecordSemanticTypeAssociationCommand>>());

        Assert.NotNull(result);
    }

    private static SemanticTypeAssociator Target(
        ICommandHandler<IRecordSemanticTypeAssociationCommand> recorder)
    {
        return new SemanticTypeAssociator(recorder);
    }
}
