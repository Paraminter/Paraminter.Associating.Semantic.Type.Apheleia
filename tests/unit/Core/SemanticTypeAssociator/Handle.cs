namespace Paraminter.Associating.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Models;
using Paraminter.Cqs.Handlers;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullCommand_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void DifferentNumberOfParametersAndArguments_HandlesError()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([Mock.Of<ITypeSymbol>()]);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Once());
    }

    [Fact]
    public void NoParametersOrArguments_PairsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([]);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Never());

        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Never());
    }

    [Fact]
    public void SameNumberOfParametersAndArguments_AssociatesAllPairwise()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();

        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup((command) => command.Data.Parameters).Returns([parameter1, parameter2]);
        commandMock.Setup((command) => command.Data.Arguments).Returns([argument1, argument2]);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Never());

        Fixture.PairerMock.Verify(PairArgumentExpression(parameter1, argument1), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameter2, argument2), Times.Once());
        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Exactly(2));
    }

    private static Expression<Action<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>>> PairArgumentExpression(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol)
    {
        return (associator) => associator.Handle(It.Is(MatchPairArgumentCommand(parameterSymbol, argumentSymbol)));
    }

    private static Expression<Func<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>, bool>> MatchPairArgumentCommand(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol)
    {
        return (command) => MatchParameter(parameterSymbol, command.Parameter) && MatchArgumentData(argumentSymbol, command.ArgumentData);
    }

    private static bool MatchParameter(
        ITypeParameterSymbol parameterSymbol,
        ITypeParameter parameter)
    {
        return ReferenceEquals(parameterSymbol, parameter.Symbol);
    }

    private static bool MatchArgumentData(
        ITypeSymbol argumentSymbol,
        ISemanticTypeArgumentData argumentData)
    {
        return ReferenceEquals(argumentSymbol, argumentData.Symbol);
    }

    private void Target(
        IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
