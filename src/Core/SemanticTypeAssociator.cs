namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;
using Paraminter.Semantic.Type.Apheleia.Common;
using Paraminter.Semantic.Type.Apheleia.Models;

using System;

/// <summary>Associates semantic type arguments.</summary>
public sealed class SemanticTypeAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>>
{
    private readonly ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>> Recorder;
    private readonly ICommandHandler<IInvalidateArgumentAssociationsRecordCommand> Invalidator;

    /// <summary>Instantiates a <see cref="SemanticTypeAssociator"/>, associating semantic type arguments.</summary>
    /// <param name="recorder">Records associated semantic type arguments.</param>
    /// <param name="invalidator">Invalidates the record of associated semantic type arguments.</param>
    public SemanticTypeAssociator(
        ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>> recorder,
        ICommandHandler<IInvalidateArgumentAssociationsRecordCommand> invalidator)
    {
        Recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
        Invalidator = invalidator ?? throw new ArgumentNullException(nameof(invalidator));
    }

    void ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>>.Handle(
        IAssociateArgumentsCommand<IAssociateSemanticTypeData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (command.Data.Parameters.Count != command.Data.Arguments.Count)
        {
            Invalidator.Handle(InvalidateArgumentAssociationsRecordCommand.Instance);

            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            var parameter = new TypeParameter(command.Data.Parameters[i]);
            var argumentData = new SemanticTypeArgumentData(command.Data.Arguments[i]);

            var recordCommand = new RecordSemanticTypeAssociationCommand(parameter, argumentData);

            Recorder.Handle(recordCommand);
        }
    }
}
