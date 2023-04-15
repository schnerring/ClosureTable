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

        var entities = new List<TestEntity>
        {
            // A
            new(null, "A", new TestRelationshipProperties(1)),
            // B
            new(null, "B", new TestRelationshipProperties(2)),
            // C
            new(null, "C", new TestRelationshipProperties(3)),
            // D
            new(null, "D", new TestRelationshipProperties(4)),
            // E
            new(null, "E", new TestRelationshipProperties(5)),
            // F
            new(null, "F", new TestRelationshipProperties(6)),
            // G
            new(null, "G", new TestRelationshipProperties(7)),
            // H
            new(null, "H", new TestRelationshipProperties(8))
        };
        // I > J > K > L
        var entityI = new TestEntity(null, "I", new TestRelationshipProperties(9));
        var entityJ = new TestEntity(entityI, "J", new TestRelationshipProperties(10));
        var entityK = new TestEntity(entityJ, "K", new TestRelationshipProperties(11));
        var entityL = new TestEntity(entityK, "L", new TestRelationshipProperties(12));
        entities.Add(entityL);
        // I > M
        var entityM = new TestEntity(entityI, "M", new TestRelationshipProperties(13));
        entities.Add(entityM);
        // I > N
        var entityN = new TestEntity(entityI, "N", new TestRelationshipProperties(14));
        entities.Add(entityN);
        // I > O
        var entityO = new TestEntity(entityI, "O", new TestRelationshipProperties(15));
        entities.Add(entityO);

        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
}
