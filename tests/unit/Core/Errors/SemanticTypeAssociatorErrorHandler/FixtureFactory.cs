namespace Paraminter.Associating.Semantic.Type.Apheleia.Errors;

using Moq;

using Paraminter.Associating.Semantic.Type.Apheleia.Errors.Commands;
using Paraminter.Cqs.Handlers;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> differentNumberOfArgumentsAndParametersMock = new();

        SemanticTypeAssociatorErrorHandler sut = new(differentNumberOfArgumentsAndParametersMock.Object);

        return new Fixture(sut, differentNumberOfArgumentsAndParametersMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ISemanticTypeAssociatorErrorHandler Sut;

        private readonly Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> DifferentNumberOfArgumentsAndParametersMock;

        public Fixture(
            ISemanticTypeAssociatorErrorHandler sut,
            Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> differentNumberOfArgumentsAndParametersMock)
        {
            Sut = sut;

            DifferentNumberOfArgumentsAndParametersMock = differentNumberOfArgumentsAndParametersMock;
        }

        ISemanticTypeAssociatorErrorHandler IFixture.Sut => Sut;

        Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> IFixture.DifferentNumberOfArgumentsAndParametersMock => DifferentNumberOfArgumentsAndParametersMock;
    }
}
