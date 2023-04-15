using ClosureTable.Helpers;

namespace ClosureTable.Models;

/// <summary>
///     Ancestor-descendant relationship entity for self-referencing entity types.
/// </summary>
/// <typeparam name="TEntity">Entity type the ancestor-descendant relationship targets.</typeparam>
/// <typeparam name="TKey">Primary key type of target type.</typeparam>
/// <typeparam name="TRelationship">Ancestor-descendant relationship type.</typeparam>
public class AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
    where TKey : struct
    where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
{
    private readonly TEntity? _ancestor;
    private readonly TEntity? _descendant;

    public AncestorDescendantRelationship(TEntity ancestor, TEntity descendant, int depth) : this()
    {
        _ancestor = ancestor;
        _descendant = descendant;
        Depth = depth;
    }

    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    protected AncestorDescendantRelationship()
    {
        AncestorId = default!;
        _ancestor = default!;

        DescendantId = default!;
        _descendant = default!;
    }

    public TKey AncestorId { get; }
    public TEntity Ancestor => _ancestor.AssertNavigationLoaded(nameof(Ancestor));

    public TKey DescendantId { get; }
    public TEntity Descendant => _descendant.AssertNavigationLoaded(nameof(Descendant));

    public int Depth { get; }
}
