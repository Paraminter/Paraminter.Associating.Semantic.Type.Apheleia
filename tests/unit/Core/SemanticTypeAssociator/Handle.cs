namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Semantic.Type.Apheleia.Models;

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
        Mock<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([Mock.Of<ITypeSymbol>()]);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Once());
    }

    [Fact]
    public void NoParametersOrArguments_AssociatesNone()
    {
        Mock<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([]);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Never());

        Fixture.IndividualAssociatorMock.Verify(static (associator) => associator.Handle(It.IsAny<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Never());
    }

    [Fact]
    public void SameNumberOfParametersAndArguments_AssociatesAllPairwise()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();

        Mock<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup((command) => command.Data.Parameters).Returns([parameter1, parameter2]);
        commandMock.Setup((command) => command.Data.Arguments).Returns([argument1, argument2]);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Never());

        Fixture.IndividualAssociatorMock.Verify(static (associator) => associator.Handle(It.IsAny<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Exactly(2));
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameter1, argument1), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameter2, argument2), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>>> AssociateIndividualExpression(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol)
    {
        return (associator) => associator.Handle(It.Is(MatchAssociateIndividualCommand(parameterSymbol, argumentSymbol)));
    }

    private static Expression<Func<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>, bool>> MatchAssociateIndividualCommand(
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
        IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
