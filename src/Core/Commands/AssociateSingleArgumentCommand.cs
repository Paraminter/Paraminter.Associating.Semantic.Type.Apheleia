namespace Paraminter.Semantic.Type.Apheleia.Commands;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Commands;
using Paraminter.Parameters.Type.Models;

internal sealed class AssociateSingleArgumentCommand
    : IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>
{
    private readonly ITypeParameter Parameter;
    private readonly ISemanticTypeArgumentData ArgumentData;

    public AssociateSingleArgumentCommand(
        ITypeParameter parameter,
        ISemanticTypeArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    ITypeParameter IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>.Parameter => Parameter;
    ISemanticTypeArgumentData IAssociateSingleArgumentCommand<ITypeParameter, ISemanticTypeArgumentData>.ArgumentData => ArgumentData;
}
