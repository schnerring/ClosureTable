namespace ClosureTable.Models;

public interface IRelationshipFactory<in TEntity, TKey, out TRelationship, in TRelationshipProperties>
    where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
    where TKey : struct
    where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
    where TRelationshipProperties : class
{
    static abstract TRelationship CreateRelationship(
        TEntity ancestor,
        TEntity descendant,
        int depth,
        TRelationshipProperties properties);
}
