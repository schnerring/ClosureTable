using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public class TestContext : DbContext
{
    public TestContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities =>
        Set<TestEntity>();

    public DbSet<AncestorDescendantRelationship<TestEntity, Guid>> TestRelationships =>
        Set<AncestorDescendantRelationship<TestEntity, Guid>>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TestEntityConfiguration());
    }
}
