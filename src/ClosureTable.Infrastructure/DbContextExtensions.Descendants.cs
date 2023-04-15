using System.Linq.Expressions;
using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure;

public static partial class DbContextExtensions
{
    private static Expression<Func<AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>, bool>>
        DescendantsPredicate<TEntity, TKey, TRelationship, TRelationshipProperties>(TKey id, bool includeSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TRelationshipProperties : class
    {
        var minDepth = includeSelf ? 0 : 1;
        return relationship => relationship.AncestorId.Equals(id) &&
                               relationship.Depth >= minDepth;
    }

    public static bool HasDescendants<TEntity, TKey, TRelationship, TRelationshipProperties>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TRelationshipProperties : class
    {
        return @this
            .Set<TRelationship>()
            .AsNoTracking()
            .Any(DescendantsPredicate<TEntity, TKey, TRelationship, TRelationshipProperties>(id, false));
    }

    public static async Task<bool> HasDescendantsAsync<TEntity, TKey, TRelationship, TRelationshipProperties>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TRelationshipProperties : class
    {
        return await @this
            .Set<TRelationship>()
            .AsNoTracking()
            .AnyAsync(DescendantsPredicate<TEntity, TKey, TRelationship, TRelationshipProperties>(id, false), cancellationToken);
    }

    public static int DescendantsCount<TEntity, TKey, TRelationship, TRelationshipProperties>(
        this DbContext @this,
        TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TRelationshipProperties : class
    {
        return @this
            .Set<TRelationship>()
            .AsNoTracking()
            .Count(DescendantsPredicate<TEntity, TKey, TRelationship, TRelationshipProperties>(id, false));
    }

    public static async Task<int> DescendantsCountAsync<TEntity, TKey, TRelationship, TRelationshipProperties>(
        this DbContext @this,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TRelationshipProperties : class
    {
        return await @this
            .Set<TRelationship>()
            .AsNoTracking()
            .CountAsync(DescendantsPredicate<TEntity, TKey, TRelationship, TRelationshipProperties>(id, false), cancellationToken);
    }

    public static IQueryable<SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>> DescendantsOf<TEntity, TKey, TRelationship, TRelationshipProperties>(
        this DbContext @this,
        TKey id,
        bool withSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TKey : struct
        where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
        where TRelationshipProperties : class
    {
        return @this
            .Set<TRelationship>()
            .Where(DescendantsPredicate<TEntity, TKey, TRelationship, TRelationshipProperties>(id, withSelf))
            .Include(relationship => relationship.Descendant)
            .Select(relationship => relationship.Descendant);
    }
}
