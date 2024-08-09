namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;
using Paraminter.Semantic.Type.Apheleia.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>> recorderMock = new();
        Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> invalidatorMock = new();

        SemanticTypeAssociator sut = new(recorderMock.Object, invalidatorMock.Object);

        return new Fixture(sut, recorderMock, invalidatorMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> Sut;

        private readonly Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>> RecorderMock;
        private readonly Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> InvalidatorMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> sut,
            Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>> recorderMock,
            Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> invalidatorMock)
        {
            Sut = sut;

            RecorderMock = recorderMock;
            InvalidatorMock = invalidatorMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>>> IFixture.RecorderMock => RecorderMock;
        Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> IFixture.InvalidatorMock => InvalidatorMock;
    }
}
