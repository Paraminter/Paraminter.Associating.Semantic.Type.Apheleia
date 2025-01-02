namespace Paraminter.Associating.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associating.Semantic.Type.Apheleia.Errors;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullPairer_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<ISemanticTypeAssociatorErrorHandler>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullErrorHandler_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>>(), Mock.Of<ISemanticTypeAssociatorErrorHandler>());

        Assert.NotNull(result);
    }

    private static SemanticTypeAssociator Target(
        ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>> pairer,
        ISemanticTypeAssociatorErrorHandler errorHandler)
    {
        return new SemanticTypeAssociator(pairer, errorHandler);
    }
}
