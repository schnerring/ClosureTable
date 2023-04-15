using ClosureTable.Helpers;

namespace ClosureTable.Models;

/// <summary>
///     Ancestor-descendant relationship entity for self-referencing entity types.
/// </summary>
/// <typeparam name="TEntity">Entity type the relationship targets.</typeparam>
/// <typeparam name="TKey">Primary key type of target type.</typeparam>
/// <typeparam name="TRelationship">Relationship type.</typeparam>
/// <typeparam name="TRelationshipProperties">Additional relationship properties.</typeparam>
public class AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
    where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
    where TKey : struct
    where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
    where TRelationshipProperties : class
{
    private readonly TEntity? _ancestor;
    private readonly TEntity? _descendant;

    public AncestorDescendantRelationship(TEntity ancestor, TEntity descendant, int depth, TRelationshipProperties properties) : this()
    {
        _ancestor = ancestor;
        _descendant = descendant;
        Depth = depth;
        Properties = properties;
    }

    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    protected AncestorDescendantRelationship()
    {
        AncestorId = default!;
        _ancestor = default!;

        DescendantId = default!;
        _descendant = default!;

        Properties = default!;
    }

    public TKey AncestorId { get; }
    public TEntity Ancestor => _ancestor.AssertNavigationLoaded(nameof(Ancestor));

    public TKey DescendantId { get; }
    public TEntity Descendant => _descendant.AssertNavigationLoaded(nameof(Descendant));

    public int Depth { get; }

    public TRelationshipProperties Properties { get; }
}
