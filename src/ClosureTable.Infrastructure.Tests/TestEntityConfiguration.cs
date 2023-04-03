using ClosureTable.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClosureTable.Infrastructure.Tests;

public class TestEntityConfiguration : SelfReferencingEntityConfiguration<TestEntity, Guid>
{
    public override void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        base.Configure(builder);

        builder
            .Property(entity => entity.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(entity => entity.Name);
    }
}
