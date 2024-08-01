namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Type.Apheleia.Commands;
using Paraminter.Semantic.Type.Commands;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>> Sut { get; }

    public abstract Mock<ICommandHandler<IRecordSemanticTypeAssociationCommand>> RecorderMock { get; }
}
