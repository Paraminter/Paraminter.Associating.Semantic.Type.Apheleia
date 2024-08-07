namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;
using Paraminter.Semantic.Type.Apheleia.Models;

using System;
using System.Linq;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void MethodInvocation_RecordsAll()
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

        Mock<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> commandMock = new();

        commandMock.Setup((command) => command.Data.Parameters).Returns(parameters);
        commandMock.Setup((command) => command.Data.Arguments).Returns(arguments);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.Never());

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>()), Times.Exactly(3));
        Fixture.RecorderMock.Verify(RecordExpression(parameters[0], arguments[0]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameters[1], arguments[1]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameters[2], arguments[2]), Times.Once());
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
