namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Type.Apheleia.Commands;
using Paraminter.Semantic.Type.Commands;

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
    public void DifferentNumberOfParametersAndArguments_RecordsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([Mock.Of<ITypeSymbol>()]);

        Target(commandMock.Object);

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordSemanticTypeAssociationCommand>()), Times.Never());
    }

    [Fact]
    public void NoParametersOrArguments_RecordsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.Arguments).Returns([]);

        Target(commandMock.Object);

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordSemanticTypeAssociationCommand>()), Times.Never());
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

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordSemanticTypeAssociationCommand>()), Times.Exactly(2));
        Fixture.RecorderMock.Verify(RecordExpression(parameter1, argument1), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameter2, argument2), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IRecordSemanticTypeAssociationCommand>>> RecordExpression(
        ITypeParameterSymbol parameter,
        ITypeSymbol argument)
    {
        return (recorder) => recorder.Handle(It.Is(MatchRecordCommand(parameter, argument)));
    }

    private static Expression<Func<IRecordSemanticTypeAssociationCommand, bool>> MatchRecordCommand(
        ITypeParameterSymbol parameter,
        ITypeSymbol argument)
    {
        return (command) => ReferenceEquals(command.Parameter, parameter) && ReferenceEquals(command.Argument, argument);
    }

    private void Target(
        IAssociateArgumentsCommand<IAssociateSemanticTypeData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
