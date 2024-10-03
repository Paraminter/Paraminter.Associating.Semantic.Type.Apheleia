namespace Paraminter.Associating.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Errors;
using Paraminter.Associating.Semantic.Type.Apheleia.Models;
using Paraminter.Cqs;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> pairerMock = new();
        Mock<ISemanticTypeAssociatorErrorHandler> errorHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        SemanticTypeAssociator sut = new(pairerMock.Object, errorHandlerMock.Object);

        return new Fixture(sut, pairerMock, errorHandlerMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> Sut;

        private readonly Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> PairerMock;
        private readonly Mock<ISemanticTypeAssociatorErrorHandler> ErrorHandlerMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> sut,
            Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> pairerMock,
            Mock<ISemanticTypeAssociatorErrorHandler> errorHandlerMock)
        {
            Sut = sut;

            PairerMock = pairerMock;
            ErrorHandlerMock = errorHandlerMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> IFixture.PairerMock => PairerMock;
        Mock<ISemanticTypeAssociatorErrorHandler> IFixture.ErrorHandlerMock => ErrorHandlerMock;
    }
}
