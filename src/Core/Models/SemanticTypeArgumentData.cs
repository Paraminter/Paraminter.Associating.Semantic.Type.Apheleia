namespace Paraminter.Associating.Semantic.Type.Apheleia.Models;

using Microsoft.CodeAnalysis;

using Paraminter.Arguments.Semantic.Type.Models;

internal sealed class SemanticTypeArgumentData
    : ISemanticTypeArgumentData
{
    private readonly ITypeSymbol Symbol;

    public SemanticTypeArgumentData(
        ITypeSymbol symbol)
    {
        Symbol = symbol;
    }

    ITypeSymbol ISemanticTypeArgumentData.Symbol => Symbol;
}
