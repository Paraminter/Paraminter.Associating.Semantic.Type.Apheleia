namespace Paraminter.Associators.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Associators.Semantic.Type.Apheleia.Queries;
using Paraminter.Associators.Semantic.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        SemanticTypeInvocationDataAssociator sut = new();

        return new Fixture(sut);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, ISemanticTypeInvocationDataAssociatorQueryResponseCollector> Sut;

        public Fixture(
            IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, ISemanticTypeInvocationDataAssociatorQueryResponseCollector> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSemanticTypeInvocationData>, ISemanticTypeInvocationDataAssociatorQueryResponseCollector> IFixture.Sut => Sut;
    }
}
