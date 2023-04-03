using FluentAssertions;

namespace ClosureTable.Infrastructure.Tests;

public abstract class TestsBase<TFixture> : IClassFixture<TFixture>, IDisposable where TFixture : DatabaseFixtureBase
{
    private readonly TFixture _fixture;

    protected TestsBase(TFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.Cleanup();
    }

    [Fact]
    public void Test1()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Assert
        context
            .TestEntities
            .ToList()
            .Should()
            .HaveCount(2);
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Assert
        context
            .TestEntities
            .Add(new TestEntity(null));

        context.SaveChanges();

        context
            .TestEntities
            .ToList()
            .Should()
            .HaveCount(3);
    }

    [Fact]
    public void Test3()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Assert
        context
            .TestEntities
            .ToList()
            .Should()
            .HaveCount(2);
    }
}
