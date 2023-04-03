namespace ClosureTable.Infrastructure.Tests.SqlServer;

public class Tests : TestsBase<DatabaseFixture<Tests>>
{
    public Tests(DatabaseFixture<Tests> fixture) : base(fixture)
    {
    }
}
