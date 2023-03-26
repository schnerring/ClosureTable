using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClosureTable.Infrastructure.Configuration;

public abstract class SelfReferencingEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<SelfReferencingEntity<TEntity, TKey>>
    where TEntity : SelfReferencingEntity<TEntity, TKey>
    where TKey : notnull
{
    public virtual void Configure(EntityTypeBuilder<SelfReferencingEntity<TEntity, TKey>> builder)
    {
        builder.HasKey(entity => entity.Id);
    }
}
