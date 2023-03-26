using ClosureTable.NET.Abstractions;

namespace ClosureTable.NET.Models;

public abstract class SelfReferencingEntity<TEntity, TKey> : IEntity<TKey>
    where TEntity : SelfReferencingEntity<TEntity, TKey>
    where TKey : notnull
{
    private HashSet<AncestorDescendantRelationship<TEntity, TKey>>? _ancestorRelationships;
    private HashSet<TEntity>? _ancestors;

    private HashSet<AncestorDescendantRelationship<TEntity, TKey>>? _descendantRelationships;
    private HashSet<TEntity>? _descendants;

    protected SelfReferencingEntity(TEntity parent) // : this()
    {
        _ancestorRelationships = new HashSet<AncestorDescendantRelationship<TEntity, TKey>>
        {
            // reflexive edge to self
            new(Derived, Derived, 0)
        };

        _descendantRelationships = new HashSet<AncestorDescendantRelationship<TEntity, TKey>>();

        SetParent(parent);
    }

    // // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    // private protected? SelfReferencingEntity()
    // {
    //     _ancestors = default!;
    //     _ancestorRelationships = default!;
    //
    //     _descendants = default!;
    //     _descendantRelationships = default!;
    // }

    private TEntity Derived => (TEntity)this;

    // ReSharper disable once ReplaceAutoPropertyWithComputedProperty
    public TKey Id { get; } = default!;

    public void SetParent(TEntity? parent)
    {
        if (parent == this)
            throw new InvalidOperationException("Cannot set parent to self");
    }
}
