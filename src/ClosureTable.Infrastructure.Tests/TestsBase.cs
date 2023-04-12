using FluentAssertions;

namespace ClosureTable.Infrastructure.Tests;

public abstract partial class TestsBase<TFixture> : IClassFixture<TFixture>, IAsyncLifetime where TFixture : DatabaseFixtureBase
{
    private readonly TFixture _fixture;

    protected TestsBase(TFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await _fixture.ReseedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Roots_ShouldHaveCount_Of9()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Act
        var roots = context.TestEntities.Roots<TestEntity, Guid>();

        // Assert
        roots.Should().HaveCount(9);
    }

    [Fact]
    public void Roots_ShouldContain_Node1To9()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Act
        var roots = context.TestEntities.Roots<TestEntity, Guid>();

        // Assert
        roots
            .Select(entity => entity.Name)
            .Should()
            .BeEquivalentTo("A", "B", "C", "D", "E", "F", "G", "H", "I");
    }

    [Fact]
    public void Test1()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Assert
        context
            .TestEntities
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
    [InlineData("A", 1)]
    [InlineData("B", 1)]
    [InlineData("C", 1)]
    [InlineData("D", 1)]
    [InlineData("E", 1)]
    [InlineData("F", 1)]
    [InlineData("G", 1)]
    [InlineData("H", 1)]
    [InlineData("I", 1)]
    [InlineData("J", 2)]
    [InlineData("K", 3)]
    [InlineData("L", 4)]
    [InlineData("M", 2)]
    [InlineData("N", 2)]
    [InlineData("O", 2)]
    public void GetAncestorRelationships_WithSelf_ShouldHaveExpectedCount(string entityName, int expectedAncestorCount)
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
    [InlineData("A", 0)]
    [InlineData("B", 0)]
    [InlineData("C", 0)]
    [InlineData("D", 0)]
    [InlineData("E", 0)]
    [InlineData("F", 0)]
    [InlineData("G", 0)]
    [InlineData("H", 0)]
    [InlineData("I", 0)]
    [InlineData("J", 1)]
    [InlineData("K", 2)]
    [InlineData("L", 3)]
    [InlineData("M", 1)]
    [InlineData("N", 1)]
    [InlineData("O", 1)]
    public void GetAncestorRelationships_WithoutSelf_ShouldHaveExpectedCount(string entityName, int expectedAncestorCount)
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

    [Theory]
    [InlineData("A", 1)]
    [InlineData("B", 1)]
    [InlineData("C", 1)]
    [InlineData("D", 1)]
    [InlineData("E", 1)]
    [InlineData("F", 1)]
    [InlineData("G", 1)]
    [InlineData("H", 1)]
    [InlineData("I", 7)]
    [InlineData("J", 3)]
    [InlineData("K", 2)]
    [InlineData("L", 1)]
    [InlineData("M", 1)]
    [InlineData("N", 1)]
    [InlineData("O", 1)]
    public void GetDescendantRelationships_WithSelf_ShouldHaveExpectedCount(string entityName, int expectedDescendantCount)
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var entity = context.TestEntities.First(entity => entity.Name == entityName);

        // Act
        var relationships = context
            .TestRelationships
            .GetDescendantRelationships(entity.Id, true);

        // Assert
        relationships
            .Should()
            .HaveCount(expectedDescendantCount);
    }

    [Theory]
    [InlineData("A", 0)]
    [InlineData("B", 0)]
    [InlineData("C", 0)]
    [InlineData("D", 0)]
    [InlineData("E", 0)]
    [InlineData("F", 0)]
    [InlineData("G", 0)]
    [InlineData("H", 0)]
    [InlineData("I", 6)]
    [InlineData("J", 2)]
    [InlineData("K", 1)]
    [InlineData("L", 0)]
    [InlineData("M", 0)]
    [InlineData("N", 0)]
    [InlineData("O", 0)]
    public void GetDescendantRelationships_WithoutSelf_ShouldHaveExpectedCount(string entityName, int expectedDescendantCount)
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var entity = context
            .TestEntities
            .First(entity => entity.Name == entityName);

        // Act
        var relationships = context
            .TestRelationships
            .GetDescendantRelationships(entity.Id, false);

        // Assert
        relationships
            .Should()
            .HaveCount(expectedDescendantCount);
    }
}
