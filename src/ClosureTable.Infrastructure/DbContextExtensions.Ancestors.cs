using System.Linq.Expressions;
using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure;

public static partial class DbContextExtensions
{
    private static Expression<Func<AncestorDescendantRelationship<TEntity, TKey, TRelationship>, bool>>
        AncestorsPredicate<TEntity, TKey, TRelationship>(TKey id, bool includeSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        var minDepth = includeSelf ? 0 : 1;
        return relationship => relationship.DescendantId.Equals(id) &&
                               relationship.Depth >= minDepth;
    }

    public static bool HasAncestors<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return @this
            .Set<TRelationship>()
            .AsNoTracking()
            .Any(AncestorsPredicate<TEntity, TKey, TRelationship>(id, false));
    }

    public static async Task<bool> HasAncestorsAsync<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return await @this
            .Set<TRelationship>()
            .AsNoTracking()
            .AnyAsync(AncestorsPredicate<TEntity, TKey, TRelationship>(id, false), cancellationToken);
    }

    public static int AncestorsCount<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return @this
            .Set<TRelationship>()
            .AsNoTracking()
            .Count(AncestorsPredicate<TEntity, TKey, TRelationship>(id, false));
    }

    public static async Task<int> AncestorsCountAsync<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return await @this
            .Set<TRelationship>()
            .AsNoTracking()
            .CountAsync(AncestorsPredicate<TEntity, TKey, TRelationship>(id, false), cancellationToken);
    }

    public static IQueryable<SelfReferencingEntity<TEntity, TKey, TRelationship>> AncestorsOf<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id,
        bool withSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return @this
            .Set<TRelationship>()
            .Where(AncestorsPredicate<TEntity, TKey, TRelationship>(id, withSelf))
            .Include(relationship => relationship.Ancestor)
            .Select(relationship => relationship.Ancestor);
    }
}
