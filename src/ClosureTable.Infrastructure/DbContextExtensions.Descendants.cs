using System.Linq.Expressions;
using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure;

public static partial class DbContextExtensions
{
    private static Expression<Func<AncestorDescendantRelationship<TEntity, TKey>, bool>>
        DescendantsPredicate<TEntity, TKey>(TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return relationship => relationship.AncestorId.Equals(id) && relationship.Depth > 0;
    }

    public static bool HasDescendants<TEntity, TKey>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .Any(DescendantsPredicate<TEntity, TKey>(id));
    }

    public static async Task<bool> HasDescendantsAsync<TEntity, TKey>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return await @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .AnyAsync(DescendantsPredicate<TEntity, TKey>(id), cancellationToken);
    }

    public static int DescendantsCount<TEntity, TKey>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .Count(DescendantsPredicate<TEntity, TKey>(id));
    }

    public static async Task<int> DescendantsCountAsync<TEntity, TKey>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return await @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .CountAsync(DescendantsPredicate<TEntity, TKey>(id), cancellationToken);
    }
}
