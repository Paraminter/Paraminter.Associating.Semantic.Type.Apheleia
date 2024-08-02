namespace Paraminter.Semantic.Type.Apheleia.Common;

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
