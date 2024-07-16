namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Collectors;

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
