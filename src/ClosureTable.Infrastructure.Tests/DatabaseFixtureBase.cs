using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClosureTable.Infrastructure.Tests;

public abstract class DatabaseFixtureBase : IDisposable
{
    protected static IConfiguration Configuration;

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
    }

    public void Cleanup()
    {
        using var context = CreateContext();

        context.Set<TestEntity>().RemoveRange(context.Set<TestEntity>());

        context.Set<TestEntity>().AddRange(
            new TestEntity(null),
            new TestEntity(null));

        context.SaveChanges();
    }

    public TestContext CreateContext()
    {
        return new TestContext(_options);
    }
}
