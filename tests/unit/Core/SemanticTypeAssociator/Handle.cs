namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Collectors;

using System;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullQuery_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<IInvalidatingAssociateSemanticTypeQueryResponseCollector>()));

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
        Mock<IInvalidatingAssociateSemanticTypeQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Parameters).Returns([]);
        queryMock.Setup(static (query) => query.Data.Arguments).Returns([Mock.Of<ITypeSymbol>()]);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify(static (collector) => collector.Invalidator.Invalidate(), Times.Once());
    }

    [Fact]
    public void NoParametersOrArguments_AddsNone()
    {
        Mock<IAssociateArgumentsQuery<IAssociateSemanticTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSemanticTypeQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Parameters).Returns([]);
        queryMock.Setup(static (query) => query.Data.Arguments).Returns([]);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify(static (collector) => collector.Invalidator.Invalidate(), Times.Never());
        queryResponseCollectorMock.Verify(static (collector) => collector.Associations.Add(It.IsAny<ITypeParameterSymbol>(), It.IsAny<ITypeSymbol>()), Times.Never());
    }

    [Fact]
    public void SameNumberOfParametersAndArguments_AddsAllPairwise()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();

        Mock<IAssociateArgumentsQuery<IAssociateSemanticTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSemanticTypeQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Parameters).Returns([parameter1, parameter2]);
        queryMock.Setup((query) => query.Data.Arguments).Returns([argument1, argument2]);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify(static (collector) => collector.Invalidator.Invalidate(), Times.Never());
        queryResponseCollectorMock.Verify(static (collector) => collector.Associations.Add(It.IsAny<ITypeParameterSymbol>(), It.IsAny<ITypeSymbol>()), Times.Exactly(2));
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(parameter1, argument1), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(parameter2, argument2), Times.Once());
    }

    private void Target(
        IAssociateArgumentsQuery<IAssociateSemanticTypeData> query,
        IInvalidatingAssociateSemanticTypeQueryResponseCollector queryResponseCollector)
    {
        Fixture.Sut.Handle(query, queryResponseCollector);
    }
}
