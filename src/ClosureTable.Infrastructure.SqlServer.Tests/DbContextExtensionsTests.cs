using ClosureTable.Infrastructure.Tests;

namespace ClosureTable.Infrastructure.SqlServer.Tests;

public class DbContextExtensionsTests : DbContextExtensionsTestsBase<DatabaseFixture<DbContextExtensionsTests>>
{
    public DbContextExtensionsTests(DatabaseFixture<DbContextExtensionsTests> fixture) : base(fixture)
    {
    }
}
