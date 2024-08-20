namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Errors;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullIndividualAssociator_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<ISemanticTypeAssociatorErrorHandler>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullErrorHandler_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>>(), Mock.Of<ISemanticTypeAssociatorErrorHandler>());

        Assert.NotNull(result);
    }

    private static SemanticTypeAssociator Target(
        ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>> individualAssociator,
        ISemanticTypeAssociatorErrorHandler errorHandler)
    {
        return new SemanticTypeAssociator(individualAssociator, errorHandler);
    }
}
