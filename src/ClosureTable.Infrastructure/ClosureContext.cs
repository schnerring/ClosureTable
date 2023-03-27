using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure;

public class ClosureContext<TEntity, TKey> : DbContext
    where TEntity : SelfReferencingEntity<TEntity, TKey>
    where TKey : notnull
{
    public IQueryable<TEntity> Roots => Set<TEntity>().Where(entity => entity.ParentId == null);
}
