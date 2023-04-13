using System.Linq.Expressions;
using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure;

public static partial class DbContextExtensions
{
    private static Expression<Func<AncestorDescendantRelationship<TEntity, TKey>, bool>>
        AncestorsPredicate<TEntity, TKey>(TKey id, bool includeSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        var minDepth = includeSelf ? 0 : 1;
        return relationship => relationship.DescendantId.Equals(id) &&
                               relationship.Depth >= minDepth;
    }

    public static bool HasAncestors<TEntity, TKey>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .Any(AncestorsPredicate<TEntity, TKey>(id, false));
    }

    public static async Task<bool> HasAncestorsAsync<TEntity, TKey>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return await @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .AnyAsync(AncestorsPredicate<TEntity, TKey>(id, false), cancellationToken);
    }

    public static int AncestorsCount<TEntity, TKey>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .Count(AncestorsPredicate<TEntity, TKey>(id, false));
    }

    public static async Task<int> AncestorsCountAsync<TEntity, TKey>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return await @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .AsNoTracking()
            .CountAsync(AncestorsPredicate<TEntity, TKey>(id, false), cancellationToken);
    }
}
