using ClosureTable.NET.Helpers;

namespace ClosureTable.NET.Models;

/// <summary>
///     Ancestor-descendant relationship entity for self-referencing entity types.
/// </summary>
/// <typeparam name="TEntity">Target type of ancestor-descendant relationship.</typeparam>
/// <typeparam name="TKey">Primary key type of target type.</typeparam>
public class AncestorDescendantRelationship<TEntity, TKey>
    where TEntity : SelfReferencingEntity<TEntity, TKey>
    where TKey : notnull
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
    private AncestorDescendantRelationship()
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
