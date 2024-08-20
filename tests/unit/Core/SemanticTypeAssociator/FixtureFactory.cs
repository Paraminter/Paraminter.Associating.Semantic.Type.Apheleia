namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Errors;
using Paraminter.Semantic.Type.Apheleia.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> individualAssociatorMock = new();
        Mock<ISemanticTypeAssociatorErrorHandler> errorHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        SemanticTypeAssociator sut = new(individualAssociatorMock.Object, errorHandlerMock.Object);

        return new Fixture(sut, individualAssociatorMock, errorHandlerMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> Sut;

        private readonly Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> IndividualAssociatorMock;
        private readonly Mock<ISemanticTypeAssociatorErrorHandler> ErrorHandlerMock;

        public Fixture(
            ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> sut,
            Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> individualAssociatorMock,
            Mock<ISemanticTypeAssociatorErrorHandler> errorHandlerMock)
        {
            Sut = sut;

            IndividualAssociatorMock = individualAssociatorMock;
            ErrorHandlerMock = errorHandlerMock;
        }

        ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> IFixture.IndividualAssociatorMock => IndividualAssociatorMock;
        Mock<ISemanticTypeAssociatorErrorHandler> IFixture.ErrorHandlerMock => ErrorHandlerMock;
    }
}
