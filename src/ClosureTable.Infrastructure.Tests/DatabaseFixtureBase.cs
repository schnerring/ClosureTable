using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClosureTable.Infrastructure.Tests;

public abstract class DatabaseFixtureBase : IAsyncLifetime
{
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
    }

    protected static IConfiguration Configuration { get; }

    public async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
    }

    public TestContext CreateContext()
    {
        return new TestContext(_options);
    }

    public async Task ReseedAsync()
    {
        await ResetDatabaseAsync();
        await SeedAsync();
    }

    protected abstract Task ResetDatabaseAsync();

    private async Task SeedAsync()
    {
        await using var context = CreateContext();

        // 1
        await context.TestEntities.AddAsync(new TestEntity(null, "1"));
        // 2
        await context.TestEntities.AddAsync(new TestEntity(null, "2"));
        // 3
        await context.TestEntities.AddAsync(new TestEntity(null, "3"));
        // 4
        await context.TestEntities.AddAsync(new TestEntity(null, "4"));
        // 5
        await context.TestEntities.AddAsync(new TestEntity(null, "5"));
        // 6
        await context.TestEntities.AddAsync(new TestEntity(null, "6"));
        // 7
        await context.TestEntities.AddAsync(new TestEntity(null, "7"));
        // 8
        await context.TestEntities.AddAsync(new TestEntity(null, "8"));
        // 9 > 10 > 11 > 12
        var entity9 = new TestEntity(null, "9");
        var entity10 = new TestEntity(entity9, "10");
        var entity11 = new TestEntity(entity10, "11");
        var entity12 = new TestEntity(entity11, "12");
        await context.TestEntities.AddAsync(entity12);
        // 9 > 13
        var entity13 = new TestEntity(entity9, "13");
        await context.TestEntities.AddAsync(entity13);
        // 9 > 14
        var entity14 = new TestEntity(entity9, "14");
        await context.TestEntities.AddAsync(entity14);
        // 9 > 15
        var entity15 = new TestEntity(entity9, "15");
        await context.TestEntities.AddAsync(entity15);

        await context.SaveChangesAsync();
    }
}
