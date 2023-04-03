using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

public static class TestEntityExtensions
{
    public static IQueryable<AncestorDescendantRelationship<TEntity, TKey>> GetAncestorRelationships<TEntity, TKey>(
        this IQueryable<AncestorDescendantRelationship<TEntity, TKey>> @this,
        TKey id,
        bool includeSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        var query = @this.Where(relationship => relationship.DescendantId.Equals(id));

        if (!includeSelf)
            query = query.Where(relationship => relationship.Depth > 0);

        return query;
    }
}
