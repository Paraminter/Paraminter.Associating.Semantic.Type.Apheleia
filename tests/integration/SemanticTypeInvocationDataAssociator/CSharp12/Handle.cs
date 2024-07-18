namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Collectors;

using System.Linq;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void MethodInvocation_AssociatesAll()
    {
        var source = """
            public class Foo
            {
                public void Invoke()
                {
                    Method<int, string, bool>();
                }

                public void Method<T1, T2, T3>() { }
            }
            """;

        var compilation = CompilationFactory.Create(source);

        var type = compilation.GetTypeByMetadataName("Foo")!;
        var method = type.GetMembers().OfType<IMethodSymbol>().Single((symbol) => symbol.Name == "Method");

        var syntaxTree = compilation.SyntaxTrees[0];
        var semanticModel = compilation.GetSemanticModel(syntaxTree);

        var invokeMethod = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single((method) => method.Identifier.Text == "Invoke");
        var methodInvocation = invokeMethod.DescendantNodes().OfType<InvocationExpressionSyntax>().Single();

        var arguments = ((IMethodSymbol)semanticModel.GetSymbolInfo(methodInvocation).Symbol!).TypeArguments;

        Mock<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>> queryMock = new();
        Mock<IInvalidatingSemanticTypeAssociationQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.UnassociatedInvocationData.Parameters).Returns(method.TypeParameters);
        queryMock.Setup((query) => query.UnassociatedInvocationData.Arguments).Returns(arguments);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(method.TypeParameters[0], arguments[0]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(method.TypeParameters[1], arguments[1]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(method.TypeParameters[2], arguments[2]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(It.IsAny<ITypeParameterSymbol>(), It.IsAny<ITypeSymbol>()), Times.Exactly(3));
    }

    private void Target(
        IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData> query,
        IInvalidatingSemanticTypeAssociationQueryResponseCollector queryResponseCollector)
    {
        Fixture.Sut.Handle(query, queryResponseCollector);
    }
}
