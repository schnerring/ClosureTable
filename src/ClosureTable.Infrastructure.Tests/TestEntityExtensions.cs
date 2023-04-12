using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

public static class TestEntityExtensions
{
    public static IQueryable<TEntity> Roots<TEntity, TKey>(this IQueryable<TEntity> @this)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return @this.Where(entity => entity.ParentId.Equals(null));
    }

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

    public static IQueryable<AncestorDescendantRelationship<TEntity, TKey>> GetDescendantRelationships<TEntity, TKey>(
        this IQueryable<AncestorDescendantRelationship<TEntity, TKey>> @this,
        TKey id,
        bool includeSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        var query = @this.Where(relationship => relationship.AncestorId.Equals(id));

        if (!includeSelf)
            query = query.Where(relationship => relationship.Depth > 0);

        return query;
    }
}
