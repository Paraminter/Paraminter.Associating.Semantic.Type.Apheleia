namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;
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
    public void DifferentNumberOfParametersAndArguments_Invalidates()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([Mock.Of<ITypeSymbol>()]);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.AtLeastOnce());
    }

    [Fact]
    public void NoParametersOrArguments_RecordsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([]);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.Never());

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Never());
    }

    [Fact]
    public void SameNumberOfParametersAndArguments_RecordsAllPairwise()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();

        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> commandMock = new();

        commandMock.Setup((command) => command.Data.Parameters).Returns([parameter1, parameter2]);
        commandMock.Setup((command) => command.Data.Arguments).Returns([argument1, argument2]);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.Never());

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Exactly(2));
        Fixture.RecorderMock.Verify(RecordExpression(parameter1, argument1), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameter2, argument2), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>>> RecordExpression(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol)
    {
        return (recorder) => recorder.Handle(It.Is(MatchRecordCommand(parameterSymbol, argumentSymbol)));
    }

    private static Expression<Func<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>, bool>> MatchRecordCommand(
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
        IAssociateArgumentsCommand<IAssociateSemanticTypeData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
