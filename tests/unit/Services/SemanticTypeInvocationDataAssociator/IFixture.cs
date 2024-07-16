namespace Paraminter.Associators.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Associators.Semantic.Type.Apheleia.Queries;
using Paraminter.Associators.Semantic.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

internal interface IFixture
{
    public abstract IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, ISemanticTypeInvocationDataAssociatorQueryResponseCollector> Sut { get; }
}
