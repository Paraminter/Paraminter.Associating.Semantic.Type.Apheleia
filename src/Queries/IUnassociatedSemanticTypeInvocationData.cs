namespace Paraminter.Associators.Semantic.Type.Simple.Queries;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;

/// <summary>Represents unassociated semantic data about type arguments and type parameters.</summary>
public interface IUnassociatedSemanticTypeInvocationData
{
    /// <summary>The type parameters of the invocation.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The type arguments of the invocation.</summary>
    public abstract IReadOnlyList<ITypeSymbol> Arguments { get; }
}
