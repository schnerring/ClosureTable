using ClosureTable.Helpers;

namespace ClosureTable.Models;

public class SelfReferencingEntity<TSelf, TKey>
    where TSelf : SelfReferencingEntity<TSelf, TKey>
    where TKey : notnull
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

    public IReadOnlyCollection<AncestorDescendantRelationship<TSelf, TKey>> AncestorRelationships =>
        _ancestorRelationships.AssertNavigationLoaded(nameof(AncestorRelationships));

    public IReadOnlyCollection<TSelf> Descendants =>
        _descendants.AssertNavigationLoaded(nameof(Descendants));

    public IReadOnlyCollection<AncestorDescendantRelationship<TSelf, TKey>> DescendantRelationships =>
        _descendantRelationships.AssertNavigationLoaded(nameof(DescendantRelationships));

    // public int Position { get; }

    public void SetParent(TSelf? parent)
    {
        if (parent == this)
            throw new InvalidOperationException("Cannot set parent to self");

        Parent = parent;
    }
}
