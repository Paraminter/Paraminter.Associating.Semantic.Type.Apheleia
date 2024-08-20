namespace Paraminter.Semantic.Type.Apheleia.Errors;

using Paraminter.Cqs.Handlers;
using Paraminter.Semantic.Type.Apheleia.Errors.Commands;

using System;

/// <inheritdoc cref="ISemanticTypeAssociatorErrorHandler"/>
public sealed class SemanticTypeAssociatorErrorHandler
    : ISemanticTypeAssociatorErrorHandler
{
    private readonly ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> DifferentNumberOfArgumentsAndParameters;

    /// <summary>Instantiates a handler of errors encountered when associating semantic attribute constructor arguments.</summary>
    /// <param name="differentNumberOfArgumentsAndParameters">Handles there being a different number of arguments and parameters.</param>
    public SemanticTypeAssociatorErrorHandler(
        ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> differentNumberOfArgumentsAndParameters)
    {
        DifferentNumberOfArgumentsAndParameters = differentNumberOfArgumentsAndParameters ?? throw new ArgumentNullException(nameof(differentNumberOfArgumentsAndParameters));
    }

    ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> ISemanticTypeAssociatorErrorHandler.DifferentNumberOfArgumentsAndParameters => DifferentNumberOfArgumentsAndParameters;
}
