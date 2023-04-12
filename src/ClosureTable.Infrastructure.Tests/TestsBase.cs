using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public abstract partial class TestsBase<TFixture> : IClassFixture<TFixture>, IAsyncLifetime
    where TFixture : DatabaseFixtureBase
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
    public void Roots_OfTestData_ShouldHaveCountOf9()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        // Act
        var roots = context.TestEntities.Roots<TestEntity, Guid>();

        // Assert
        roots.Should().HaveCount(9);
    }

    [Fact]
    public void Roots_OfTestData_ShouldBeExpectedEntities()
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

    [Theory]
    [InlineData("A", false)]
    [InlineData("B", false)]
    [InlineData("C", false)]
    [InlineData("D", false)]
    [InlineData("E", false)]
    [InlineData("F", false)]
    [InlineData("G", false)]
    [InlineData("H", false)]
    [InlineData("I", true)]
    [InlineData("J", true)]
    [InlineData("K", true)]
    [InlineData("L", false)]
    [InlineData("M", false)]
    [InlineData("N", false)]
    [InlineData("O", false)]
    public void HasDescendants_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = context.HasDescendants<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("A", false)]
    [InlineData("B", false)]
    [InlineData("C", false)]
    [InlineData("D", false)]
    [InlineData("E", false)]
    [InlineData("F", false)]
    [InlineData("G", false)]
    [InlineData("H", false)]
    [InlineData("I", true)]
    [InlineData("J", true)]
    [InlineData("K", true)]
    [InlineData("L", false)]
    [InlineData("M", false)]
    [InlineData("N", false)]
    [InlineData("O", false)]
    public async Task HasDescendantsAsync_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = await context.HasDescendantsAsync<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
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
    public void DescendantsCount_OfTestDataEntity_ShouldBeExpected(string entityName, int expected)
    {
        // Arrange
        using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = context.DescendantsCount<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
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
    public async Task DescendantsCountAsync_OfTestDataEntity_ShouldBeExpected(string entityName, int expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = await context.DescendantsCountAsync<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("A", false)]
    [InlineData("B", false)]
    [InlineData("C", false)]
    [InlineData("D", false)]
    [InlineData("E", false)]
    [InlineData("F", false)]
    [InlineData("G", false)]
    [InlineData("H", false)]
    [InlineData("I", false)]
    [InlineData("J", true)]
    [InlineData("K", true)]
    [InlineData("L", true)]
    [InlineData("M", true)]
    [InlineData("N", true)]
    [InlineData("O", true)]
    public void HasAncestors_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = context.HasAncestors<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("A", false)]
    [InlineData("B", false)]
    [InlineData("C", false)]
    [InlineData("D", false)]
    [InlineData("E", false)]
    [InlineData("F", false)]
    [InlineData("G", false)]
    [InlineData("H", false)]
    [InlineData("I", false)]
    [InlineData("J", true)]
    [InlineData("K", true)]
    [InlineData("L", true)]
    [InlineData("M", true)]
    [InlineData("N", true)]
    [InlineData("O", true)]
    public async Task HasAncestorsAsync_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = await context.HasAncestorsAsync<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
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
    public void AncestorsCount_OfTestDataEntity_ShouldBeExpected(string entityName, int expected)
    {
        // Arrange
        using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = context.AncestorsCount<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
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
    public async Task AncestorsCountAsync_OfTestDataEntity_ShouldBeExpected(string entityName, int expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var sut = context
            .TestEntities
            .AsNoTracking()
            .First(entity => entity.Name == entityName);

        // Act
        var actual = await context.AncestorsCountAsync<TestEntity, Guid>(sut.Id);

        // Assert
        actual.Should().Be(expected);
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
    public void GetAncestorRelationships_WithoutSelf_ShouldHaveExpectedCount(
        string entityName,
        int expectedAncestorCount)
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
    public void GetDescendantRelationships_WithSelf_ShouldHaveExpectedCount(
        string entityName,
        int expectedDescendantCount)
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
    public void GetDescendantRelationships_WithoutSelf_ShouldHaveExpectedCount(
        string entityName,
        int expectedDescendantCount)
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
