namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Semantic.Type.Apheleia.Models;

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

        Mock<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> commandMock = new();

        commandMock.Setup((command) => command.Data.Parameters).Returns(parameters);
        commandMock.Setup((command) => command.Data.Arguments).Returns(arguments);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Never());

        Fixture.IndividualAssociatorMock.Verify(static (associator) => associator.Handle(It.IsAny<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Exactly(3));
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameters[0], arguments[0]), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameters[1], arguments[1]), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameters[2], arguments[2]), Times.Once());
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
