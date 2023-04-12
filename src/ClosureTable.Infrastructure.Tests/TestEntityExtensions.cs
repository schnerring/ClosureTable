using System.Linq.Expressions;
using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public static class TestEntityExtensions
{
    public static IQueryable<TEntity> Roots<TEntity, TKey>(this IQueryable<TEntity> @this)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return @this.Where(entity => entity.ParentId.Equals(null));
    }

    // TODO: private query builder
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

    // TODO: private query builder
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

    private static Expression<Func<AncestorDescendantRelationship<TEntity, TKey>, bool>>
        AncestorsPredicate<TEntity, TKey>(TKey id)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return relationship => relationship.DescendantId.Equals(id) && relationship.Depth > 0;
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
            .Any(AncestorsPredicate<TEntity, TKey>(id));
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
            .AnyAsync(AncestorsPredicate<TEntity, TKey>(id), cancellationToken);
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
            .Count(AncestorsPredicate<TEntity, TKey>(id));
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
            .CountAsync(AncestorsPredicate<TEntity, TKey>(id), cancellationToken);
    }
}
