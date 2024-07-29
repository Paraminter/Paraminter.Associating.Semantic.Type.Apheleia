namespace Paraminter.Semantic.Type.Apheleia.Common;

using Microsoft.CodeAnalysis;

using Paraminter.Semantic.Type.Commands;

internal sealed class AddSemanticTypeAssociationCommand
    : IAddSemanticTypeAssociationCommand
{
    private readonly ITypeParameterSymbol Parameter;
    private readonly ITypeSymbol Argument;

    public AddSemanticTypeAssociationCommand(
        ITypeParameterSymbol parameter,
        ITypeSymbol argument)
    {
        Parameter = parameter;
        Argument = argument;
    }

    ITypeParameterSymbol IAddSemanticTypeAssociationCommand.Parameter => Parameter;
    ITypeSymbol IAddSemanticTypeAssociationCommand.Argument => Argument;
}
