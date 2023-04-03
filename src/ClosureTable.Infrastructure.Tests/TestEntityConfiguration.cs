using ClosureTable.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClosureTable.Infrastructure.Tests;

public class TestEntityConfiguration : SelfReferencingEntityConfiguration<TestEntity, Guid>
{
    public override void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.Ignore(entity => entity.ParentId);
        builder.Ignore(entity => entity.Parent);

        builder.Ignore(entity => entity.Children);

        builder
            .Property(entity => entity.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        base.Configure(builder);
    }
}
