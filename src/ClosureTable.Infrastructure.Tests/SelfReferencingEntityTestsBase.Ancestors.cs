using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public abstract partial class SelfReferencingEntityTestsBase<TFixture>
{
    [Fact]
    public void AncestorsWithSelf_OfNewEntity_ShouldOnlyContainSelf()
    {
        // Arrange
        static TestEntity Act() => new(null, "foo", new TestRelationshipProperties(1));

        // Act
        var newEntity = Act();

        // Assert
        newEntity.Ancestors.Should().ContainSingle(ancestor => ancestor == newEntity);
    }

    [Fact]
    public void AncestorsWithoutSelf_OfNewEntity_ShouldBeEmpty()
    {
        // Arrange
        static TestEntity Act() => new(null, "foo", new TestRelationshipProperties(1));

        // Act
        var newEntity = Act();

        // Assert
        newEntity.AncestorsWithoutSelf.Should().BeEmpty();
    }

    [Theory]
    [InlineData("A", new[] { "A" })]
    [InlineData("B", new[] { "B" })]
    [InlineData("C", new[] { "C" })]
    [InlineData("D", new[] { "D" })]
    [InlineData("E", new[] { "E" })]
    [InlineData("F", new[] { "F" })]
    [InlineData("G", new[] { "G" })]
    [InlineData("H", new[] { "H" })]
    [InlineData("I", new[] { "I" })]
    [InlineData("J", new[] { "I", "J" })]
    [InlineData("K", new[] { "I", "J", "K" })]
    [InlineData("L", new[] { "I", "J", "K", "L" })]
    [InlineData("M", new[] { "I", "M" })]
    [InlineData("N", new[] { "I", "N" })]
    [InlineData("O", new[] { "I", "O" })]
    public async Task AncestorsWithSelf_OfTestDataEntity_ShouldBeExpectedAncestors(
        string entityName,
        string[] expectedAncestorNames)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Ancestors)
            .FirstAsync(e => e.Name == entityName);

        var expectedAncestors = await context
            .TestEntities
            .Where(e => expectedAncestorNames.Contains(e.Name))
            .ToListAsync();

        // Act
        var actualAncestors = entity.Ancestors;

        // Assert
        actualAncestors
            .Should()
            .BeEquivalentTo(expectedAncestors);
    }

    [Theory]
    [InlineData("A", new string[0])]
    [InlineData("B", new string[0])]
    [InlineData("C", new string[0])]
    [InlineData("D", new string[0])]
    [InlineData("E", new string[0])]
    [InlineData("F", new string[0])]
    [InlineData("G", new string[0])]
    [InlineData("H", new string[0])]
    [InlineData("I", new string[0])]
    [InlineData("J", new[] { "I" })]
    [InlineData("K", new[] { "I", "J" })]
    [InlineData("L", new[] { "I", "J", "K" })]
    [InlineData("M", new[] { "I" })]
    [InlineData("N", new[] { "I" })]
    [InlineData("O", new[] { "I" })]
    public async Task AncestorsWithoutSelf_OfTestDataEntity_ShouldBeExpectedAncestors(
        string entityName,
        string[] expectedAncestorNames)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Ancestors)
            .FirstAsync(e => e.Name == entityName);

        var expectedAncestors = await context
            .TestEntities
            .Where(e => expectedAncestorNames.Contains(e.Name))
            .ToListAsync();

        // Act
        var actualAncestors = entity.AncestorsWithoutSelf;

        // Assert
        actualAncestors
            .Should()
            .BeEquivalentTo(expectedAncestors);
    }
}
