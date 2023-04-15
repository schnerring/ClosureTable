using ClosureTable.Helpers;

namespace ClosureTable.Models;

public abstract class SelfReferencingEntity<TEntity, TKey, TRelationship>
    where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>,
    IRelationshipFactory<TEntity, TKey, TRelationship>
    where TKey : struct
    where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
{
    private readonly HashSet<TRelationship>? _ancestorRelationships;
    private readonly HashSet<TEntity>? _ancestors;

    private readonly HashSet<TEntity>? _children;

    private readonly HashSet<TRelationship>? _descendantRelationships;
    private readonly HashSet<TEntity>? _descendants;

    protected SelfReferencingEntity(TEntity? parent) : this()
    {
        var reflexiveRelationship = TEntity.CreateRelationship((TEntity)this, (TEntity)this, 0, null);

        _ancestors = new HashSet<TEntity> { (TEntity)this };
        _ancestorRelationships = new HashSet<TRelationship> { reflexiveRelationship };

        _descendants = new HashSet<TEntity> { (TEntity)this };
        _descendantRelationships = new HashSet<TRelationship> { reflexiveRelationship };

        _children = new HashSet<TEntity>();

        // if (position is > 0)
        //     Position = position.Value;

        SetParent(parent);
    }

    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    protected SelfReferencingEntity()
    {
        Id = default!;

        ParentId = default!;
        Parent = default!;

        _children = default!;

        _ancestors = default!;
        _ancestorRelationships = default!;

        _descendants = default!;
        _descendantRelationships = default!;
    }

    public TKey Id { get; }

    public TKey? ParentId { get; }
    public TEntity? Parent { get; private set; }

    public IReadOnlyCollection<TEntity> Children =>
        _children.AssertNavigationLoaded(nameof(Children));

    public bool IsParent => Children.Any();

    public bool IsRoot => Parent is null;

    public IReadOnlyCollection<TEntity> Ancestors =>
        _ancestors.AssertNavigationLoaded(nameof(Ancestors));

    public IReadOnlyCollection<TEntity> AncestorsWithoutSelf =>
        Ancestors.Where(ancestor => ancestor != this).ToList();

    public IReadOnlyCollection<TRelationship> AncestorRelationships =>
        _ancestorRelationships.AssertNavigationLoaded(nameof(AncestorRelationships));

    public IReadOnlyCollection<TEntity> Descendants =>
        _descendants.AssertNavigationLoaded(nameof(Descendants));

    public IReadOnlyCollection<TEntity> DescendantsWithoutSelf =>
        Descendants.Where(descendant => descendant != this).ToList();

    public IReadOnlyCollection<TRelationship> DescendantRelationships =>
        _descendantRelationships.AssertNavigationLoaded(nameof(DescendantRelationships));

    // public int Position { get; }

    public void SetParent(TEntity? parent)
    {
        if (parent == this)
            throw new InvalidOperationException("Cannot set parent to self");

        Parent = parent;

        if (parent is not null)
        {
            var ancestorRelationships =
                _ancestorRelationships.AssertNavigationLoaded(nameof(AncestorRelationships));

            // Copy all ancestor relationships of the parent.
            // Set the copy's descendant to this, and increment it's depth by one.
            foreach (var parentAncestorRelationship in parent.AncestorRelationships)
                ancestorRelationships.Add(
                    TEntity.CreateRelationship(
                        parentAncestorRelationship.Ancestor,
                        (TEntity)this,
                        parentAncestorRelationship.Depth + 1,
                        null)); // TODO
        }

        // Re-build ancestor relationships of children recursively
        foreach (var child in Children)
            child.SetParent((TEntity)this);
    }
}
