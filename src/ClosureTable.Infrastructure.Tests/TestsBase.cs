using FluentAssertions;

namespace ClosureTable.Infrastructure.Tests;

public abstract class TestsBase<TFixture> : IClassFixture<TFixture>, IDisposable where TFixture : DatabaseFixtureBase
{
    private readonly TestContext _context;

    protected TestsBase(TFixture fixture)
    {
        _context = fixture.CreateContext();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void Test1()
    {
        _context
            .TestEntities
            .ToList()
            .Should()
            .HaveCount(c => c > 0);
    }
}
