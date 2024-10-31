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
using System.Threading;
using System.Threading.Tasks;

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

    async Task ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData>>.Handle(
        IAssociateArgumentsCommand<IAssociateSemanticTypeArgumentsData> command,
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (command.Data.Parameters.Count != command.Data.Arguments.Count)
        {
            await ErrorHandler.DifferentNumberOfArgumentsAndParameters.Handle(HandleDifferentNumberOfArgumentsAndParametersCommand.Instance, cancellationToken).ConfigureAwait(false);

            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            await PairArgument(command.Data.Parameters[i], command.Data.Arguments[i], cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task PairArgument(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol,
        CancellationToken cancellationToken)
    {
        var parameter = new TypeParameter(parameterSymbol);
        var argumentData = new SemanticTypeArgumentData(argumentSymbol);

        var command = new PairArgumentCommand(parameter, argumentData);

        await Pairer.Handle(command, cancellationToken).ConfigureAwait(false);
    }
}
