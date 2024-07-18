namespace Paraminter.Semantic.Type.Apheleia.Queries;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;

/// <summary>Represents data used to associate semantic type arguments.</summary>
public interface IAssociateSemanticTypeData
{
    /// <summary>The type parameters.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The semantic type arguments.</summary>
    public abstract IReadOnlyList<ITypeSymbol> Arguments { get; }
}
