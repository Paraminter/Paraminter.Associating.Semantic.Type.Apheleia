namespace Paraminter.Semantic.Type.Apheleia.Common;

using Microsoft.CodeAnalysis;

using Paraminter.Semantic.Type.Commands;

internal sealed class RecordSemanticTypeAssociationCommand
    : IRecordSemanticTypeAssociationCommand
{
    private readonly ITypeParameterSymbol Parameter;
    private readonly ITypeSymbol Argument;

    public RecordSemanticTypeAssociationCommand(
        ITypeParameterSymbol parameter,
        ITypeSymbol argument)
    {
        Parameter = parameter;
        Argument = argument;
    }

    ITypeParameterSymbol IRecordSemanticTypeAssociationCommand.Parameter => Parameter;
    ITypeSymbol IRecordSemanticTypeAssociationCommand.Argument => Argument;
}
