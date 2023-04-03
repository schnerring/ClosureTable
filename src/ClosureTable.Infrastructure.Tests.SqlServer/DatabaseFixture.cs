using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClosureTable.Infrastructure.Tests.SqlServer;

public class DatabaseFixture<TTest> : DatabaseFixtureBase where TTest : IClassFixture<DatabaseFixture<TTest>>
{
    // Default connection string for local development
    // Requires SQL Server Express LocalDB
    private const string DefaultConnectionStringTemplate =
        @"Server=(LocalDb)\MSSQLLocalDB;Database={databasePlaceholder};Integrated Security=True";

    // ReSharper disable once StaticMemberInGenericType
    // Get a separate field for each test class
    private static readonly string ConnectionString;

    static DatabaseFixture()
    {
        var connectionStringTemplate = Configuration.GetConnectionString("TestDatabase");

        if (string.IsNullOrWhiteSpace(connectionStringTemplate))
            connectionStringTemplate = DefaultConnectionStringTemplate;

        if (!connectionStringTemplate.Contains("{databasePlaceholder}"))
            throw new InvalidOperationException("Connection string template must contain {databasePlaceholder}");

        ConnectionString = connectionStringTemplate.Replace("{databasePlaceholder}", typeof(TTest).FullName);
    }

    public DatabaseFixture() : base(
        new DbContextOptionsBuilder<TestContext>()
            .UseSqlServer(ConnectionString)
            .Options)
    {
    }
}
