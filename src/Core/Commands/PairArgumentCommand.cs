namespace Paraminter.Associating.Semantic.Type.Apheleia.Commands;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

internal sealed class PairArgumentCommand
    : IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>
{
    private readonly ITypeParameter Parameter;
    private readonly ISemanticTypeArgumentData ArgumentData;

    public PairArgumentCommand(
        ITypeParameter parameter,
        ISemanticTypeArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    ITypeParameter IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>.Parameter => Parameter;
    ISemanticTypeArgumentData IPairArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>.ArgumentData => ArgumentData;
}
