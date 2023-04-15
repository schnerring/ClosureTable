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
    [InlineData("I", true)]
    [InlineData("J", true)]
    [InlineData("K", true)]
    [InlineData("L", false)]
    [InlineData("M", false)]
    [InlineData("N", false)]
    [InlineData("O", false)]
    public async Task HasDescendants_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .AsNoTracking()
            .FirstAsync(e => e.Name == entityName);

        // Act
        // ReSharper disable once MethodHasAsyncOverload
        var actual = context.HasDescendants<TestEntity, Guid, TestRelationship>(entity.Id);

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

        var entity = await context
            .TestEntities
            .AsNoTracking()
            .FirstAsync(e => e.Name == entityName);

        // Act
        var actual = await context.HasDescendantsAsync<TestEntity, Guid, TestRelationship>(entity.Id);

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
    public async Task DescendantsCount_OfTestDataEntity_ShouldBeExpected(string entityName, int expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .AsNoTracking()
            .FirstAsync(e => e.Name == entityName);

        // Act
        // ReSharper disable once MethodHasAsyncOverload
        var actual = context.DescendantsCount<TestEntity, Guid, TestRelationship>(entity.Id);

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

        var entity = await context
            .TestEntities
            .AsNoTracking()
            .FirstAsync(e => e.Name == entityName);

        // Act
        var actual = await context.DescendantsCountAsync<TestEntity, Guid, TestRelationship>(entity.Id);

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
    [InlineData("I", true, new[] { "I", "J", "K", "L", "M", "N", "O" })]
    [InlineData("I", false, new[] { "J", "K", "L", "M", "N", "O" })]
    [InlineData("J", true, new[] { "J", "K", "L" })]
    [InlineData("J", false, new[] { "K", "L" })]
    [InlineData("K", true, new[] { "K", "L" })]
    [InlineData("K", false, new[] { "L" })]
    [InlineData("L", true, new[] { "L" })]
    [InlineData("L", false, new string[0])]
    [InlineData("M", true, new[] { "M" })]
    [InlineData("M", false, new string[0])]
    [InlineData("N", true, new[] { "N" })]
    [InlineData("N", false, new string[0])]
    [InlineData("O", true, new[] { "O" })]
    [InlineData("O", false, new string[0])]
    public async Task DescendantsOf_OfTestDataEntity_ShouldBeExpectedDescendants(
        string entityName,
        bool withSelf,
        string[] expectedDescendantNames)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .AsNoTracking()
            .FirstAsync(e => e.Name == entityName);

        var expected = await context
            .TestEntities
            .Where(e => expectedDescendantNames.Contains(e.Name))
            .ToListAsync();

        // Act
        var actual = await context
            .DescendantsOf<TestEntity, Guid, TestRelationship>(entity.Id, withSelf)
            .ToListAsync();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
