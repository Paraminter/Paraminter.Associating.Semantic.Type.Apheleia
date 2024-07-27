namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Collectors;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        SemanticTypeAssociator sut = new();

        return new Fixture(sut);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseCollector> Sut;

        public Fixture(
            IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseCollector> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseCollector> IFixture.Sut => Sut;
    }
}
