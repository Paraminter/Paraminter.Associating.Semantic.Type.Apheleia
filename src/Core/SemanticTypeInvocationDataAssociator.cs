namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Collectors;

using System;

/// <summary>Associates semantic type argument.</summary>
public sealed class SemanticTypeInvocationDataAssociator
    : IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, IInvalidatingSemanticTypeAssociationQueryResponseCollector>
{
    /// <summary>Instantiates a <see cref="SemanticTypeInvocationDataAssociator"/>, associating semantic type arguments.</summary>
    public SemanticTypeInvocationDataAssociator() { }

    void IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, IInvalidatingSemanticTypeAssociationQueryResponseCollector>.Handle(
        IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData> query,
        IInvalidatingSemanticTypeAssociationQueryResponseCollector queryResponseCollector)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseCollector is null)
        {
            throw new ArgumentNullException(nameof(queryResponseCollector));
        }

        if (query.UnassociatedInvocationData.Parameters.Count != query.UnassociatedInvocationData.Arguments.Count)
        {
            queryResponseCollector.Invalidator.Invalidate();

            return;
        }

        for (var i = 0; i < query.UnassociatedInvocationData.Parameters.Count; i++)
        {
            var parameter = query.UnassociatedInvocationData.Parameters[i];
            var argumentData = query.UnassociatedInvocationData.Arguments[i];

            queryResponseCollector.Associations.Add(parameter, argumentData);
        }
    }
}
