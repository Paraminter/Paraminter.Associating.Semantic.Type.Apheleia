namespace Paraminter.Semantic.Type.Apheleia.Queries;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;

/// <summary>Represents unassociated semantic type arguments of an invocation.</summary>
public interface IUnassociatedSemanticTypeInvocationData
{
    /// <summary>The type parameters of the invocation.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The type arguments of the invocation.</summary>
    public abstract IReadOnlyList<ITypeSymbol> Arguments { get; }
}
