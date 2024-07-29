namespace Paraminter.Semantic.Type.Apheleia;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Type.Apheleia.Queries;
using Paraminter.Semantic.Type.Queries.Handlers;

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
        private readonly IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseHandler> Sut;

        public Fixture(
            IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseHandler> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticTypeData>, IInvalidatingAssociateSemanticTypeQueryResponseHandler> IFixture.Sut => Sut;
    }
}
