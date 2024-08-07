namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullRecorder_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullInvalidator_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>>(), Mock.Of<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>>());

        Assert.NotNull(result);
    }

    private static SemanticTypeAssociator Target(
        ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>> recorder,
        ICommandHandler<IInvalidateArgumentAssociationsRecordCommand> invalidator)
    {
        return new SemanticTypeAssociator(recorder, invalidator);
    }
}
