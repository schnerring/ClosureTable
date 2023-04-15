using ClosureTable.Models;
using Respawn.Graph;

namespace ClosureTable.Infrastructure.Tests;

public static class TestEntityExtensions
{
    public static IQueryable<TEntity> Roots<TEntity, TKey, TRelationship, TRelationshipProperties>(this IQueryable<TEntity> @this)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TRelationshipProperties : class
    {
        return @this.Where(entity => entity.ParentId.Equals(null));
    }
}
