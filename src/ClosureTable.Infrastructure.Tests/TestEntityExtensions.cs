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
}
