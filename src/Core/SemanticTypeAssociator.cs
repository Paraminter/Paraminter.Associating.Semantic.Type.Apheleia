namespace Paraminter.Semantic.Type.Apheleia;

using Microsoft.CodeAnalysis;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Commands;
using Paraminter.Semantic.Type.Apheleia.Errors;
using Paraminter.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Semantic.Type.Apheleia.Models;

using System;

/// <summary>Associates semantic type arguments with parameters.</summary>
public sealed class SemanticTypeAssociator
    : ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>>
{
    private readonly ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>> IndividualAssociator;
    private readonly ISemanticTypeAssociatorErrorHandler ErrorHandler;

    /// <summary>Instantiates an associator of semantic type arguments with parameters.</summary>
    /// <param name="individualAssociator">Associates individual semantic type arguments with parameters.</param>
    /// <param name="errorHandler">Handles encountered errors.</param>
    public SemanticTypeAssociator(
        ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>> individualAssociator,
        ISemanticTypeAssociatorErrorHandler errorHandler)
    {
        IndividualAssociator = individualAssociator ?? throw new ArgumentNullException(nameof(individualAssociator));
        ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
    }

    void ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData>>.Handle(
        IAssociateAllArgumentsCommand<IAssociateAllSemanticTypeArgumentsData> command)
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
            AssociateArgument(command.Data.Parameters[i], command.Data.Arguments[i]);
        }
    }

    private void AssociateArgument(
        ITypeParameterSymbol parameterSymbol,
        ITypeSymbol argumentSymbol)
    {
        var parameter = new TypeParameter(parameterSymbol);
        var argumentData = new SemanticTypeArgumentData(argumentSymbol);

        var associateIndividualCommand = new AssociateSingleArgumentCommand(parameter, argumentData);

        IndividualAssociator.Handle(associateIndividualCommand);
    }
}
