namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Collectors;

using System;

/// <summary>Associates semantic type argument.</summary>
public sealed class SemanticTypeAssociator
    : IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseCollector>
{
    /// <summary>Instantiates a <see cref="SemanticTypeAssociator"/>, associating semantic type arguments.</summary>
    public SemanticTypeAssociator() { }

    void IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseCollector>.Handle(
        IAssociateArgumentsQuery<IAssociateSemanticTypeData> query,
        IInvalidatingAssociateSemanticTypeQueryResponseCollector queryResponseCollector)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseCollector is null)
        {
            throw new ArgumentNullException(nameof(queryResponseCollector));
        }

        if (query.Data.Parameters.Count != query.Data.Arguments.Count)
        {
            queryResponseCollector.Invalidator.Invalidate();

            return;
        }

        for (var i = 0; i < query.Data.Parameters.Count; i++)
        {
            var parameter = query.Data.Parameters[i];
            var argumentData = query.Data.Arguments[i];

            queryResponseCollector.Associations.Add(parameter, argumentData);
        }
    }
}
