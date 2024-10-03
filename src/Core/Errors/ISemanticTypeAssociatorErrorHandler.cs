namespace Paraminter.Associating.Semantic.Type.Apheleia.Errors;

using Paraminter.Associating.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Cqs;

/// <summary>Handles errors encountered when associating semantic type arguments with parameters.</summary>
public interface ISemanticTypeAssociatorErrorHandler
{
    /// <summary>Handles there being a different number of arguments and parameters.</summary>
    public abstract ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> DifferentNumberOfArgumentsAndParameters { get; }
}
