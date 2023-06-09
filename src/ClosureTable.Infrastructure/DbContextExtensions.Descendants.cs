﻿using System.Linq.Expressions;
using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure;

public static partial class DbContextExtensions
{
    private static Expression<Func<AncestorDescendantRelationship<TEntity, TKey>, bool>>
        DescendantsPredicate<TEntity, TKey>(TKey id, bool includeSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        var minDepth = includeSelf ? 0 : 1;
        return relationship => relationship.AncestorId.Equals(id) &&
                               relationship.Depth >= minDepth;
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
            .Any(DescendantsPredicate<TEntity, TKey>(id, false));
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
            .AnyAsync(DescendantsPredicate<TEntity, TKey>(id, false), cancellationToken);
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
            .Count(DescendantsPredicate<TEntity, TKey>(id, false));
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
            .CountAsync(DescendantsPredicate<TEntity, TKey>(id, false), cancellationToken);
    }

    public static IQueryable<SelfReferencingEntity<TEntity, TKey>> DescendantsOf<TEntity, TKey>(
        this DbContext @this,
        TKey id,
        bool withSelf)
        where TEntity : SelfReferencingEntity<TEntity, TKey>
        where TKey : struct
    {
        return @this
            .Set<AncestorDescendantRelationship<TEntity, TKey>>()
            .Where(DescendantsPredicate<TEntity, TKey>(id, withSelf))
            .Include(relationship => relationship.Descendant)
            .Select(relationship => relationship.Descendant);
    }
}
