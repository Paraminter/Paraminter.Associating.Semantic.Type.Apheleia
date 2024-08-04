namespace Paraminter.Semantic.Type.Apheleia.Common;

using Microsoft.CodeAnalysis;

using Paraminter.Parameters.Type.Models;

internal sealed class TypeParameter
    : ITypeParameter
{
    private ITypeParameterSymbol Symbol = null!;

    public void Change(
        ITypeParameterSymbol symbol)
    {
        Symbol = symbol;
    }

    ITypeParameterSymbol ITypeParameter.Symbol => Symbol;
}
