namespace Paraminter.Semantic.Type.Apheleia.Common;

using Paraminter.Arguments.Semantic.Type.Models;
using Paraminter.Associators.Commands;
using Paraminter.Parameters.Type.Models;

internal sealed class RecordSemanticTypeAssociationCommand
    : IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>
{
    private readonly ITypeParameter Parameter;
    private readonly ISemanticTypeArgumentData ArgumentData;

    public RecordSemanticTypeAssociationCommand(
        ITypeParameter parameter,
        ISemanticTypeArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    ITypeParameter IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>.Parameter => Parameter;
    ISemanticTypeArgumentData IRecordArgumentAssociationCommand<ITypeParameter, ISemanticTypeArgumentData>.ArgumentData => ArgumentData;
}
