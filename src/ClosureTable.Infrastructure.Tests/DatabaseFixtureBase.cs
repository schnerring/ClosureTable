using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClosureTable.Infrastructure.Tests;

public abstract class DatabaseFixtureBase : IDisposable
{
    protected static IConfiguration Configuration { get; }

    private readonly DbContextOptions _options;

    static DatabaseFixtureBase()
    {
        Configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
    }

    protected DatabaseFixtureBase(DbContextOptions options)
    {
        _options = options;

        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Cleanup();
    }

    public void Dispose()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();

        GC.SuppressFinalize(this);
    }

    public void Cleanup()
    {
        using var context = CreateContext();

        context.TestEntities.RemoveRange(context.TestEntities);

        // 1
        context.TestEntities.Add(new TestEntity(null));
        // 2
        context.TestEntities.Add(new TestEntity(null));
        // 3
        context.TestEntities.Add(new TestEntity(null));
        // 4
        context.TestEntities.Add(new TestEntity(null));
        // 5
        context.TestEntities.Add(new TestEntity(null));
        // 6
        context.TestEntities.Add(new TestEntity(null));
        // 7
        context.TestEntities.Add(new TestEntity(null));
        // 8
        context.TestEntities.Add(new TestEntity(null));
        // 9 > 10 > 11 > 12
        var entity9 = new TestEntity(null);
        var entity10 = new TestEntity(entity9);
        var entity11 = new TestEntity(entity10);
        var entity12 = new TestEntity(entity11);
        context.TestEntities.Add(entity12);
        // 9 > 13
        var entity13 = new TestEntity(entity9);
        context.TestEntities.Add(entity13);
        // 9 > 14
        var entity14 = new TestEntity(entity9);
        context.TestEntities.Add(entity14);
        // 9 > 15
        var entity15 = new TestEntity(entity9);
        context.TestEntities.Add(entity15);

        context.SaveChanges();
    }

    public TestContext CreateContext()
    {
        return new TestContext(_options);
    }
}
