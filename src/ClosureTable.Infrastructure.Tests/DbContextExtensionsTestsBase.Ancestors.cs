using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public abstract partial class DbContextExtensionsTestsBase<TFixture>
{
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
    public async Task HasAncestors_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = context
            .TestEntities
            .AsNoTracking()
            .First(e => e.Name == entityName);

        // Act
        // ReSharper disable once MethodHasAsyncOverload
        var actual = context.HasAncestors<TestEntity, Guid>(entity.Id);

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

        var entity = context
            .TestEntities
            .AsNoTracking()
            .First(e => e.Name == entityName);

        // Act
        var actual = await context.HasAncestorsAsync<TestEntity, Guid>(entity.Id);

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
    public async Task AncestorsCount_OfTestDataEntity_ShouldBeExpected(string entityName, int expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = context
            .TestEntities
            .AsNoTracking()
            .First(e => e.Name == entityName);

        // Act
        // ReSharper disable once MethodHasAsyncOverload
        var actual = context.AncestorsCount<TestEntity, Guid>(entity.Id);

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

        var entity = context
            .TestEntities
            .AsNoTracking()
            .First(e => e.Name == entityName);

        // Act
        var actual = await context.AncestorsCountAsync<TestEntity, Guid>(entity.Id);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("A", true, new[] { "A" })]
    [InlineData("A", false, new string[0])]
    [InlineData("B", true, new[] { "B" })]
    [InlineData("B", false, new string[0])]
    [InlineData("C", true, new[] { "C" })]
    [InlineData("C", false, new string[0])]
    [InlineData("D", true, new[] { "D" })]
    [InlineData("D", false, new string[0])]
    [InlineData("E", true, new[] { "E" })]
    [InlineData("E", false, new string[0])]
    [InlineData("F", true, new[] { "F" })]
    [InlineData("F", false, new string[0])]
    [InlineData("G", true, new[] { "G" })]
    [InlineData("G", false, new string[0])]
    [InlineData("H", true, new[] { "H" })]
    [InlineData("H", false, new string[0])]
    [InlineData("I", true, new[] { "I" })]
    [InlineData("I", false, new string[0])]
    [InlineData("J", true, new[] { "I", "J" })]
    [InlineData("J", false, new[] { "I" })]
    [InlineData("K", true, new[] { "I", "J", "K" })]
    [InlineData("K", false, new[] { "I", "J" })]
    [InlineData("L", true, new[] { "I", "J", "K", "L" })]
    [InlineData("L", false, new[] { "I", "J", "K" })]
    [InlineData("M", true, new[] { "I", "M" })]
    [InlineData("M", false, new[] { "I" })]
    [InlineData("N", true, new[] { "I", "N" })]
    [InlineData("N", false, new[] { "I" })]
    [InlineData("O", true, new[] { "I", "O" })]
    [InlineData("O", false, new[] { "I" })]
    public async Task AncestorsOf_OfTestDataEntity_ShouldBeExpectedAncestors(
        string entityName,
        bool withSelf,
        string[] expectedAncestorNames)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = context
            .TestEntities
            .AsNoTracking()
            .First(e => e.Name == entityName);

        var expected = await context
            .TestEntities
            .Where(e => expectedAncestorNames.Contains(e.Name))
            .ToListAsync();

        // Act
        var actual = await context
            .AncestorsOf<TestEntity, Guid>(entity.Id, withSelf)
            .ToListAsync();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
