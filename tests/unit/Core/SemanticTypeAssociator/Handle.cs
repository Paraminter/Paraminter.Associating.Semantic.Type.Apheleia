namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Invalidation.Commands;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Commands;
using Paraminter.Semantic.Type.Queries.Handlers;

using System;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullQuery_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<IInvalidatingAssociateSemanticTypeQueryResponseHandler>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullQueryResponseCollector_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<IAssociateArgumentsQuery<IAssociateSemanticTypeData>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void DifferentNumberOfParametersAndArguments_Invalidates()
    {
        Mock<IAssociateArgumentsQuery<IAssociateSemanticTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSemanticTypeQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Parameters).Returns([]);
        queryMock.Setup(static (query) => query.Data.Arguments).Returns([Mock.Of<ITypeSymbol>()]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.Invalidator.Handle(It.IsAny<IInvalidateQueryResponseCommand>()), Times.Once());
    }

    [Fact]
    public void NoParametersOrArguments_AddsNone()
    {
        Mock<IAssociateArgumentsQuery<IAssociateSemanticTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSemanticTypeQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Parameters).Returns([]);
        queryMock.Setup(static (query) => query.Data.Arguments).Returns([]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.Invalidator.Handle(It.IsAny<IInvalidateQueryResponseCommand>()), Times.Never());
        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddSemanticTypeAssociationCommand>()), Times.Never());
    }

    [Fact]
    public void SameNumberOfParametersAndArguments_AddsAllPairwise()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();

        Mock<IAssociateArgumentsQuery<IAssociateSemanticTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSemanticTypeQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Parameters).Returns([parameter1, parameter2]);
        queryMock.Setup((query) => query.Data.Arguments).Returns([argument1, argument2]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.Invalidator.Handle(It.IsAny<IInvalidateQueryResponseCommand>()), Times.Never());
        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddSemanticTypeAssociationCommand>()), Times.Exactly(2));
        queryResponseHandlerMock.Verify(AssociationExpression(parameter1, argument1), Times.Once());
        queryResponseHandlerMock.Verify(AssociationExpression(parameter2, argument2), Times.Once());
    }

    private static Expression<Action<IInvalidatingAssociateSemanticTypeQueryResponseHandler>> AssociationExpression(
        ITypeParameterSymbol parameter,
        ITypeSymbol argument)
    {
        return (handler) => handler.AssociationCollector.Handle(It.Is(MatchAssociationCommand(parameter, argument)));
    }

    private static Expression<Func<IAddSemanticTypeAssociationCommand, bool>> MatchAssociationCommand(
        ITypeParameterSymbol parameter,
        ITypeSymbol argument)
    {
        return (command) => ReferenceEquals(command.Parameter, parameter) && ReferenceEquals(command.Argument, argument);
    }

    private void Target(
        IAssociateArgumentsQuery<IAssociateSemanticTypeData> query,
        IInvalidatingAssociateSemanticTypeQueryResponseHandler queryResponseHandler)
    {
        Fixture.Sut.Handle(query, queryResponseHandler);
    }
}
