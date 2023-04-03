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

        GC.SuppressFinalize(this);
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

        // Act
        context
            .TestEntities
            .Add(new TestEntity(null));
        context.SaveChanges();

        // Assert
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

        // Act
        var firstEntity = context.TestEntities.First();
        var relationships = context
            .TestRelationships
            .GetAncestorRelationships(firstEntity.Id, true);

        // Assert
        relationships
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void Test4()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Act
        var firstEntity = context.TestEntities.First();
        var relationships = context
            .TestRelationships
            .GetAncestorRelationships(firstEntity.Id, false);

        // Assert
        relationships
            .Should()
            .HaveCount(0);
    }
}
