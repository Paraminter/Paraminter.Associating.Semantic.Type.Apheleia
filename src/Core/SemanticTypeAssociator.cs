namespace Paraminter.Associating.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Errors;
using Paraminter.Associating.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Associating.Semantic.Type.Apheleia.Models;
using Paraminter.Cqs;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;

/// <summary>Associates semantic type arguments with parameters.</summary>
public sealed class SemanticTypeAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>>
{
    private readonly ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>> Pairer;
    private readonly ISemanticTypeAssociatorErrorHandler ErrorHandler;

    /// <summary>Instantiates an associator of semantic type arguments with parameters.</summary>
    /// <param name="pairer">Pairs semantic type arguments with parameters.</param>
    /// <param name="errorHandler">Handles encountered errors.</param>
    public SemanticTypeAssociator(
        ICommandHandler<IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>> pairer,
        ISemanticTypeAssociatorErrorHandler errorHandler)
    {
        Pairer = pairer ?? throw new ArgumentNullException(nameof(pairer));
        ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
    }

    void ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>>.Handle(
        IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (command.Data.Parameters.Count != command.Data.Arguments.Count)
        {
            ErrorHandler.DifferentNumberOfArgumentsAndParameters.Handle(HandleDifferentNumberOfArgumentsAndParametersCommand.Instance);

            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            PairArgument(command.Data.Parameters[i], command.Data.Arguments[i]);
        }
    }

    private void PairArgument(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol)
    {
        var parameter = new TypeParameter(parameterSymbol);
        var argumentData = new SemanticTypeArgumentData(argumentSymbol);

        var command = new PairArgumentCommand(parameter, argumentData);

        Pairer.Handle(command);
    }
}
