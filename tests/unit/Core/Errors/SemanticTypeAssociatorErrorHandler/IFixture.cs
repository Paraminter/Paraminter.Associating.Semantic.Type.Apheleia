namespace Paraminter.Semantic.Type.Apheleia.Errors;

using Moq;

using Paraminter.Cqs.Handlers;
using Paraminter.Semantic.Type.Apheleia.Errors.Commands;

internal interface IFixture
{
    public abstract ISemanticTypeAssociatorErrorHandler Sut { get; }

    public abstract Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> DifferentNumberOfArgumentsAndParametersMock { get; }
}
