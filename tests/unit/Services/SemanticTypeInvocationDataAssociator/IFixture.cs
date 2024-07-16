namespace Paraminter.Associators.Semantic.Type.Simple;

using Paraminter.Associators.Queries;
using Paraminter.Associators.Semantic.Type.Queries.Collectors;
using Paraminter.Associators.Semantic.Type.Simple.Queries;
using Paraminter.Queries.Handlers;

internal interface IFixture
{
    public abstract IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, ISemanticTypeInvocationDataAssociatorQueryResponseCollector> Sut { get; }
}
