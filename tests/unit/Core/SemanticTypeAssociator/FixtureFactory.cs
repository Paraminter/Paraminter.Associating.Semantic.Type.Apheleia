namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Type.Apheleia.Commands;
using Paraminter.Semantic.Type.Commands;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IRecordSemanticTypeAssociationCommand>> recorderMock = new();

        SemanticTypeAssociator sut = new(recorderMock.Object);

        return new Fixture(sut, recorderMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> Sut;

        private readonly Mock<ICommandHandler<IRecordSemanticTypeAssociationCommand>> RecorderMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> sut,
            Mock<ICommandHandler<IRecordSemanticTypeAssociationCommand>> recorderMock)
        {
            Sut = sut;

            RecorderMock = recorderMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IRecordSemanticTypeAssociationCommand>> IFixture.RecorderMock => RecorderMock;
    }
}
