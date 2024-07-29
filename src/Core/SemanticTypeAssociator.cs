namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Type.Apheleia.Common;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Handlers;

using System;

/// <summary>Associates semantic type arguments.</summary>
public sealed class SemanticTypeAssociator
    : IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseHandler>
{
    /// <summary>Instantiates a <see cref="SemanticTypeAssociator"/>, associating semantic type arguments.</summary>
    public SemanticTypeAssociator() { }

    void IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseHandler>.Handle(
        IAssociateArgumentsQuery<IAssociateSemanticTypeData> query,
        IInvalidatingAssociateSemanticTypeQueryResponseHandler queryResponseHandler)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseHandler is null)
        {
            throw new ArgumentNullException(nameof(queryResponseHandler));
        }

        if (query.Data.Parameters.Count != query.Data.Arguments.Count)
        {
            queryResponseHandler.Invalidator.Handle(InvalidateQueryResponseCommand.Instance);

            return;
        }

        for (var i = 0; i < query.Data.Parameters.Count; i++)
        {
            var parameter = query.Data.Parameters[i];
            var argument = query.Data.Arguments[i];

            var command = new AddSemanticTypeAssociationCommand(parameter, argument);

            queryResponseHandler.AssociationCollector.Handle(command);
        }
    }
}
