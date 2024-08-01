namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Type.Apheleia.Commands;
using Paraminter.Semantic.Type.Apheleia.Common;
using Paraminter.Semantic.Type.Commands;

using System;

/// <summary>Associates semantic type arguments.</summary>
public sealed class SemanticTypeAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticTypeData>>
{
    private readonly ICommandHandler<IRecordSemanticTypeAssociationCommand> Recorder;

    /// <summary>Instantiates a <see cref="SemanticTypeAssociator"/>, associating semantic type arguments.</summary>
    /// <param name="recorder">Records associated semantic type arguments.</param>
    public SemanticTypeAssociator(
        ICommandHandler<IRecordSemanticTypeAssociationCommand> recorder)
    {
        Recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
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
            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            var parameter = command.Data.Parameters[i];
            var argument = command.Data.Arguments[i];

            var recordCommand = new RecordSemanticTypeAssociationCommand(parameter, argument);

            Recorder.Handle(recordCommand);
        }
    }
}
