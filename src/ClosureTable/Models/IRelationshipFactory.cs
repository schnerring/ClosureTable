namespace ClosureTable.Models;

public interface IRelationshipFactory<in TEntity, TKey, out TRelationship>
    where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
    where TKey : struct
    where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
{
    static abstract TRelationship CreateRelationship(
        TEntity ancestor,
        TEntity descendant,
        int depth,
        IRelationshipOptions options);
}
