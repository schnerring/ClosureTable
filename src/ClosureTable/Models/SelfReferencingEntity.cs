using ClosureTable.Helpers;

namespace ClosureTable.Models;

public class SelfReferencingEntity<TSelf, TKey>
    where TSelf : SelfReferencingEntity<TSelf, TKey>
    where TKey : struct
{
    private readonly HashSet<AncestorDescendantRelationship<TSelf, TKey>>? _ancestorRelationships;
    private readonly HashSet<TSelf>? _ancestors;

    private readonly HashSet<TSelf>? _children;

    private readonly HashSet<AncestorDescendantRelationship<TSelf, TKey>>? _descendantRelationships;
    private readonly HashSet<TSelf>? _descendants;

    public SelfReferencingEntity(TSelf? parent) : this()
    {
        _ancestorRelationships = new HashSet<AncestorDescendantRelationship<TSelf, TKey>>
        {
            // Reflexivity
            new((TSelf)this, (TSelf)this, 0)
        };

        _descendantRelationships = new HashSet<AncestorDescendantRelationship<TSelf, TKey>>();

        _children = new HashSet<TSelf>();

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
    public TSelf? Parent { get; private set; }

    public IReadOnlyCollection<TSelf> Children =>
        _children.AssertNavigationLoaded(nameof(Children));

    public IReadOnlyCollection<TSelf> Ancestors =>
        _ancestors.AssertNavigationLoaded(nameof(Ancestors));

    public IReadOnlyCollection<TSelf> AncestorsWithoutSelf =>
        Ancestors.Where(ancestor => ancestor != this).ToList();

    public IReadOnlyCollection<AncestorDescendantRelationship<TSelf, TKey>> AncestorRelationships =>
        _ancestorRelationships.AssertNavigationLoaded(nameof(AncestorRelationships));

    public IReadOnlyCollection<TSelf> Descendants =>
        _descendants.AssertNavigationLoaded(nameof(Descendants));

    public IReadOnlyCollection<TSelf> DescendantsWithoutSelf =>
        Descendants.Where(descendant => descendant != this).ToList();

    public IReadOnlyCollection<AncestorDescendantRelationship<TSelf, TKey>> DescendantRelationships =>
        _descendantRelationships.AssertNavigationLoaded(nameof(DescendantRelationships));

    // public int Position { get; }

    public void SetParent(TSelf? parent)
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
                    new AncestorDescendantRelationship<TSelf, TKey>(
                        parentAncestorRelationship.Ancestor,
                        (TSelf)this,
                        parentAncestorRelationship.Depth + 1));
        }

        // Re-build ancestor relationships of children recursively
        foreach (var child in Children)
            child.SetParent((TSelf)this);
    }
}
