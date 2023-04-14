using ClosureTable.Infrastructure.Tests;

namespace ClosureTable.Infrastructure.SqlServer.Tests;

public class Tests : TestsBase<DatabaseFixture<Tests>>
{
    public Tests(DatabaseFixture<Tests> fixture) : base(fixture)
    {
    }
}
