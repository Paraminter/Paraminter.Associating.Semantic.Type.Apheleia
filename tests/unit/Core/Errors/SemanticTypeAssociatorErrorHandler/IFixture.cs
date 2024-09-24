namespace Paraminter.Associating.Semantic.Type.Apheleia.Errors;

using Moq;

using Paraminter.Associating.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Cqs.Handlers;

internal interface IFixture
{
    public abstract ISemanticTypeAssociatorErrorHandler Sut { get; }

    public abstract Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> DifferentNumberOfArgumentsAndParametersMock { get; }
}
