using System.Linq.Expressions;
using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure;

public static partial class DbContextExtensions
{
    private static Expression<Func<AncestorDescendantRelationship<TEntity, TKey, TRelationship>, bool>>
        DescendantsPredicate<TEntity, TKey, TRelationship>(TKey id, bool includeSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        var minDepth = includeSelf ? 0 : 1;
        return relationship => relationship.AncestorId.Equals(id) &&
                               relationship.Depth >= minDepth;
    }

    public static bool HasDescendants<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return @this
            .Set<TRelationship>()
            .AsNoTracking()
            .Any(DescendantsPredicate<TEntity, TKey, TRelationship>(id, false));
    }

    public static async Task<bool> HasDescendantsAsync<TEntity, TKey, TRelationship>(
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
            .AnyAsync(DescendantsPredicate<TEntity, TKey, TRelationship>(id, false), cancellationToken);
    }

    public static int DescendantsCount<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return @this
            .Set<TRelationship>()
            .AsNoTracking()
            .Count(DescendantsPredicate<TEntity, TKey, TRelationship>(id, false));
    }

    public static async Task<int> DescendantsCountAsync<TEntity, TKey, TRelationship>(
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
            .CountAsync(DescendantsPredicate<TEntity, TKey, TRelationship>(id, false), cancellationToken);
    }

    public static IQueryable<SelfReferencingEntity<TEntity, TKey, TRelationship>> DescendantsOf<TEntity, TKey, TRelationship>(
        this DbContext @this,
        TKey id,
        bool withSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship>, IRelationshipFactory<TEntity, TKey, TRelationship>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship>
    {
        return @this
            .Set<TRelationship>()
            .Where(DescendantsPredicate<TEntity, TKey, TRelationship>(id, withSelf))
            .Include(relationship => relationship.Descendant)
            .Select(relationship => relationship.Descendant);
    }
}
