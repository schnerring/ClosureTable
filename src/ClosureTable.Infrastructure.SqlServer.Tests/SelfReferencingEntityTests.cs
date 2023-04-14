using ClosureTable.Infrastructure.Tests;

namespace ClosureTable.Infrastructure.SqlServer.Tests;

public class SelfReferencingEntityTests : SelfReferencingEntityTestsBase<DatabaseFixture<SelfReferencingEntityTests>>
{
    public SelfReferencingEntityTests(DatabaseFixture<SelfReferencingEntityTests> fixture) : base(fixture)
    {
    }
}
