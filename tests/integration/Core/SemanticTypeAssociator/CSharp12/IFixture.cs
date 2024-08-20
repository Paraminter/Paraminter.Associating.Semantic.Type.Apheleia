namespace Paraminter.Semantic.Type.Apheleia;

using Moq;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Errors;
using Paraminter.Semantic.Type.Apheleia.Models;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>> Sut { get; }

    public abstract Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>>> IndividualAssociatorMock { get; }
    public abstract Mock<ISemanticTypeAssociatorErrorHandler> ErrorHandlerMock { get; }
}
