namespace Paraminter.Semantic.Type.Apheleia.Errors;

using Paraminter.Cqs.Handlers;
using Paraminter.Semantic.Type.Apheleia.Errors.Commands;

/// <summary>Handles errors encountered when associating semantic type arguments with parameters.</summary>
public interface ISemanticTypeAssociatorErrorHandler
{
    /// <summary>Handles there being a different number of arguments and parameters.</summary>
    public abstract ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> DifferentNumberOfArgumentsAndParameters { get; }
}
