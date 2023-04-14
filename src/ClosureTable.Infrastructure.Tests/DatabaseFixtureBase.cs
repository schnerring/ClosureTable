using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

    protected DatabaseFixtureBase(DbContextOptionsBuilder builder)
    {
        _options = builder
            .LogTo(Console.WriteLine, LogLevel.Information)
            .Options;
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

        // ```goat
        // roots:  A    B    C    D    E    F    G    H    I
        //                                                 |
        //                                            .--+-+-+--.
        //                                           |   |   |   |
        //                                           J   M   N   O
        //                                           |
        //                                           K
        //                                           |
        //                                           L
        // ```

        // A
        await context.TestEntities.AddAsync(new TestEntity(null, "A"));
        // B
        await context.TestEntities.AddAsync(new TestEntity(null, "B"));
        // C
        await context.TestEntities.AddAsync(new TestEntity(null, "C"));
        // D
        await context.TestEntities.AddAsync(new TestEntity(null, "D"));
        // E
        await context.TestEntities.AddAsync(new TestEntity(null, "E"));
        // F
        await context.TestEntities.AddAsync(new TestEntity(null, "F"));
        // G
        await context.TestEntities.AddAsync(new TestEntity(null, "G"));
        // H
        await context.TestEntities.AddAsync(new TestEntity(null, "H"));
        // I > J > K > L
        var entityI = new TestEntity(null, "I");
        var entityJ = new TestEntity(entityI, "J");
        var entityK = new TestEntity(entityJ, "K");
        var entityL = new TestEntity(entityK, "L");
        await context.TestEntities.AddAsync(entityL);
        // I > M
        var entityM = new TestEntity(entityI, "M");
        await context.TestEntities.AddAsync(entityM);
        // I > N
        var entityN = new TestEntity(entityI, "N");
        await context.TestEntities.AddAsync(entityN);
        // I > O
        var entityO = new TestEntity(entityI, "O");
        await context.TestEntities.AddAsync(entityO);

        await context.SaveChangesAsync();
    }
}
