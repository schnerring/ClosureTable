namespace ClosureTable.Infrastructure.Tests.SqlServer;

public class DbContextExtensionsTests : DbContextExtensionsTestsBase<DatabaseFixture<DbContextExtensionsTests>>
{
    public DbContextExtensionsTests(DatabaseFixture<DbContextExtensionsTests> fixture) : base(fixture)
    {
    }
}
