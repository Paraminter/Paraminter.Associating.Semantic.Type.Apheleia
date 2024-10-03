namespace Paraminter.Associating.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Models;
using Paraminter.Cqs;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;
using System.Linq;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void MethodInvocation_PairsAll()
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

        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup((command) => command.Data.Parameters).Returns(parameters);
        commandMock.Setup((command) => command.Data.Arguments).Returns(arguments);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Never());

        Fixture.PairerMock.Verify(PairArgumentExpression(parameters[0], arguments[0]), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameters[1], arguments[1]), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameters[2], arguments[2]), Times.Once());
        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Exactly(3));
    }

    private static Expression<Action<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>>> PairArgumentExpression(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol)
    {
        return (handler) => handler.Handle(It.Is(MatchPairArgumentCommand(parameterSymbol, argumentSymbol)));
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
