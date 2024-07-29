namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Commands;
using Paraminter.Semantic.Type.Queries.Handlers;

using System;
using System.Linq;
using System.Linq.Expressions;

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
        var parameters = method.TypeParameters;

        var syntaxTree = compilation.SyntaxTrees[0];
        var semanticModel = compilation.GetSemanticModel(syntaxTree);

        var invokeMethod = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single(static (method) => method.Identifier.Text == "Invoke");
        var methodInvocation = invokeMethod.DescendantNodes().OfType<InvocationExpressionSyntax>().Single();

        var arguments = ((IMethodSymbol)semanticModel.GetSymbolInfo(methodInvocation).Symbol!).TypeArguments;

        Mock<IAssociateArgumentsQuery<IAssociateSemanticTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSemanticTypeQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Parameters).Returns(parameters);
        queryMock.Setup((query) => query.Data.Arguments).Returns(arguments);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddSemanticTypeAssociationCommand>()), Times.Exactly(3));
        queryResponseHandlerMock.Verify(AssociationExpression(parameters[0], arguments[0]), Times.Once());
        queryResponseHandlerMock.Verify(AssociationExpression(parameters[1], arguments[1]), Times.Once());
        queryResponseHandlerMock.Verify(AssociationExpression(parameters[2], arguments[2]), Times.Once());
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
