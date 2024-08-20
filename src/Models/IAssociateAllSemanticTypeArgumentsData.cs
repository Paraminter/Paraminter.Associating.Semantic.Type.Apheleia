namespace Paraminter.Semantic.Type.Apheleia.Models;

using Microsoft.CodeAnalysis;

using Paraminter.Models;

using System.Collections.Generic;

/// <summary>Represents data used to associate all semantic type arguments with parameters.</summary>
public interface IAssociateAllSemanticTypeArgumentsData
    : IAssociateAllArgumentsData
{
    /// <summary>The type parameters.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The semantic type arguments.</summary>
    public abstract IReadOnlyList<ITypeSymbol> Arguments { get; }
}
