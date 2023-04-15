using ClosureTable.Models;
using Respawn.Graph;

namespace ClosureTable.Infrastructure.Tests;

public static class TestEntityExtensions
{
    public static IQueryable<TEntity> Roots<TEntity, TKey, TRelationship>(this IQueryable<TEntity> @this)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return @this.Where(entity => entity.ParentId.Equals(null));
    }
}
