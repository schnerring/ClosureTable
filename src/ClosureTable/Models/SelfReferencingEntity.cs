using ClosureTable.Helpers;

namespace ClosureTable.Models;

public class SelfReferencingEntity<TEntity, TKey>
    where TEntity : SelfReferencingEntity<TEntity, TKey>
    where TKey : notnull
{
    private readonly HashSet<AncestorDescendantRelationship<TEntity, TKey>>? _ancestorRelationships;
    private readonly HashSet<TEntity>? _ancestors;

    private readonly HashSet<AncestorDescendantRelationship<TEntity, TKey>>? _descendantRelationships;
    private readonly HashSet<TEntity>? _descendants;

    public SelfReferencingEntity(TEntity? parent = null, int? position = null) : this()
    {
        Parent = parent;

        _ancestorRelationships = new HashSet<AncestorDescendantRelationship<TEntity, TKey>>
        {
            // reflexive edge to self
            new(DerivedInstance, DerivedInstance, 0)
        };

        _descendantRelationships = new HashSet<AncestorDescendantRelationship<TEntity, TKey>>();

        if (position is > 0)
            Position = position.Value;

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

    public TKey Id { get; }

    public TKey? ParentId { get; }
    public TEntity? Parent { get; }

    public IReadOnlyCollection<TEntity> Ancestors =>
        _ancestors.AssertNavigationLoaded(nameof(Ancestors));

    public IReadOnlyCollection<AncestorDescendantRelationship<TEntity, TKey>> AncestorRelationships =>
        _ancestorRelationships.AssertNavigationLoaded(nameof(AncestorRelationships));

    public IReadOnlyCollection<TEntity> Descendants =>
        _descendants.AssertNavigationLoaded(nameof(Descendants));

    public IReadOnlyCollection<AncestorDescendantRelationship<TEntity, TKey>> DescendantRelationships =>
        _ancestorRelationships.AssertNavigationLoaded(nameof(DescendantRelationships));

    public int Position { get; }

    private TEntity DerivedInstance => (TEntity)this;

    public void SetParent(TEntity? parent)
    {
        if (parent == this)
            throw new InvalidOperationException("Cannot set parent to self");
    }
}
