using ClosureTable.NET.Abstractions;
using ClosureTable.NET.Helpers;

namespace ClosureTable.NET.Models;

public abstract class SelfReferencingEntity<TEntity, TKey> : IEntity<TKey>
    where TEntity : SelfReferencingEntity<TEntity, TKey>
    where TKey : notnull
{
    private readonly HashSet<AncestorDescendantRelationship<TEntity, TKey>>? _ancestorRelationships;
    private readonly HashSet<TEntity>? _ancestors;

    private readonly HashSet<AncestorDescendantRelationship<TEntity, TKey>>? _descendantRelationships;
    private readonly HashSet<TEntity>? _descendants;

    protected SelfReferencingEntity(TEntity parent) : this()
    {
        _ancestorRelationships = new HashSet<AncestorDescendantRelationship<TEntity, TKey>>
        {
            // reflexive edge to self
            new(Derived, Derived, 0)
        };

        _descendantRelationships = new HashSet<AncestorDescendantRelationship<TEntity, TKey>>();

        SetParent(parent);
    }

    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    private SelfReferencingEntity()
    {
        Id = default!;

        _ancestors = default!;
        _ancestorRelationships = default!;

        _descendants = default!;
        _descendantRelationships = default!;
    }

    public IReadOnlyCollection<TEntity> Ancestors =>
        _ancestors.AssertNavigationLoaded(nameof(Ancestors));

    public IReadOnlyCollection<AncestorDescendantRelationship<TEntity, TKey>> AncestorRelationships =>
        _ancestorRelationships.AssertNavigationLoaded(nameof(AncestorRelationships));

    public IReadOnlyCollection<TEntity> Descendants =>
        _descendants.AssertNavigationLoaded(nameof(Descendants));

    public IReadOnlyCollection<AncestorDescendantRelationship<TEntity, TKey>> DescendantRelationships =>
        _descendantRelationships.AssertNavigationLoaded(nameof(DescendantRelationships));

    private TEntity Derived => (TEntity)this;

    public TKey Id { get; }

    public void SetParent(TEntity? parent)
    {
        if (parent == this)
            throw new InvalidOperationException("Cannot set parent to self");
    }
}
