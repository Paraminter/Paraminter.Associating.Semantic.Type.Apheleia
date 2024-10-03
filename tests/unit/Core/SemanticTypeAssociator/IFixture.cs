namespace Paraminter.Associating.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Errors;
using Paraminter.Associating.Semantic.Type.Apheleia.Models;
using Paraminter.Cqs;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>> Sut { get; }

    public abstract Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> PairerMock { get; }
    public abstract Mock<ISemanticTypeAssociatorErrorHandler> ErrorHandlerMock { get; }
}
