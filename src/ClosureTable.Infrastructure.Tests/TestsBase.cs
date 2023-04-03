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
            .HaveCount(15);
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Act
        context
            .TestEntities
            .Add(new TestEntity(null, "new"));
        context.SaveChanges();

        // Assert
        context
            .TestEntities
            .ToList()
            .Should()
            .HaveCount(16);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("2", 1)]
    [InlineData("3", 1)]
    [InlineData("4", 1)]
    [InlineData("5", 1)]
    [InlineData("6", 1)]
    [InlineData("7", 1)]
    [InlineData("8", 1)]
    [InlineData("9", 1)]
    [InlineData("10", 2)]
    [InlineData("11", 3)]
    [InlineData("12", 4)]
    [InlineData("13", 2)]
    [InlineData("14", 2)]
    [InlineData("15", 2)]
    public void Test3(string entityName, int expectedAncestorCount)
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var entity = context.TestEntities.First(entity => entity.Name == entityName);

        // Act
        var relationships = context
            .TestRelationships
            .GetAncestorRelationships(entity.Id, true);

        // Assert
        relationships
            .Should()
            .HaveCount(expectedAncestorCount);
    }

    [Theory]
    [InlineData("1", 0)]
    [InlineData("2", 0)]
    [InlineData("3", 0)]
    [InlineData("4", 0)]
    [InlineData("5", 0)]
    [InlineData("6", 0)]
    [InlineData("7", 0)]
    [InlineData("8", 0)]
    [InlineData("9", 0)]
    [InlineData("10", 1)]
    [InlineData("11", 2)]
    [InlineData("12", 3)]
    [InlineData("13", 1)]
    [InlineData("14", 1)]
    [InlineData("15", 1)]
    public void Test4(string entityName, int expectedAncestorCount)
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var entity = context.TestEntities.First(entity => entity.Name == entityName);

        // Act
        var relationships = context
            .TestRelationships
            .GetAncestorRelationships(entity.Id, false);

        // Assert
        relationships
            .Should()
            .HaveCount(expectedAncestorCount);
    }
}
