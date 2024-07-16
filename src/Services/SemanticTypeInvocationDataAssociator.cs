namespace Paraminter.Associators.Semantic.Type.Simple;

using Paraminter.Associators.Queries;
using Paraminter.Associators.Semantic.Type.Queries.Collectors;
using Paraminter.Associators.Semantic.Type.Simple.Queries;
using Paraminter.Queries.Handlers;

using System;

/// <summary>Associates semantic data about type arguments and type parameters.</summary>
public sealed class SemanticTypeInvocationDataAssociator
    : IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, ISemanticTypeInvocationDataAssociatorQueryResponseCollector>
{
    /// <summary>Instantiates a <see cref="SemanticTypeInvocationDataAssociator"/>, associating semantic data about type arguments and type parameters.</summary>
    public SemanticTypeInvocationDataAssociator() { }

    void IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, ISemanticTypeInvocationDataAssociatorQueryResponseCollector>.Handle(
        IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData> query,
        ISemanticTypeInvocationDataAssociatorQueryResponseCollector queryResponseCollector)
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
