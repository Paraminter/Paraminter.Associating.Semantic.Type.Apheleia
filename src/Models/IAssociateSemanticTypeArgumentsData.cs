namespace Paraminter.Associating.Semantic.Type.Apheleia.Models;

using Microsoft.CodeAnalysis;

using Paraminter.Associating.Models;

using System.Collections.Generic;

/// <summary>Represents data used to associate semantic type arguments with parameters.</summary>
public interface IAssociateSemanticTypeArgumentsData
    : IAssociateArgumentsData
{
    /// <summary>The type parameters.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The semantic type arguments.</summary>
    public abstract IReadOnlyList<ITypeSymbol> Arguments { get; }
}
