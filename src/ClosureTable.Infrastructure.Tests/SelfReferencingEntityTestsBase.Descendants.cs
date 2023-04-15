using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public abstract partial class SelfReferencingEntityTestsBase<TFixture>
{
    [Fact]
    public void DescendantsWithSelf_OfNewEntity_ShouldOnlyContainSelf()
    {
        // Arrange
        static TestEntity Act() => new(null, "foo", new TestRelationshipProperties(1));

        // Act
        var newEntity = Act();

        // Assert
        newEntity.Descendants.Should().ContainSingle(descendant => descendant == newEntity);
    }

    [Fact]
    public void DescendantsWithoutSelf_OfNewEntity_ShouldBeEmpty()
    {
        // Arrange
        static TestEntity Act() => new(null, "foo", new TestRelationshipProperties(1));

        // Act
        var newEntity = Act();

        // Assert
        newEntity.DescendantsWithoutSelf.Should().BeEmpty();
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
    [InlineData("I", new[] { "I", "J", "K", "L", "M", "N", "O" })]
    [InlineData("J", new[] { "J", "K", "L" })]
    [InlineData("K", new[] { "K", "L" })]
    [InlineData("L", new[] { "L" })]
    [InlineData("M", new[] { "M" })]
    [InlineData("N", new[] { "N" })]
    [InlineData("O", new[] { "O" })]
    public async Task DescendantsWithSelf_OfTestDataEntity_ShouldBeExpectedDescendants(
        string entityName,
        string[] expectedDescendantNames)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Descendants)
            .FirstAsync(e => e.Name == entityName);

        var expectedDescendants = await context
            .TestEntities
            .Where(e => expectedDescendantNames.Contains(e.Name))
            .ToListAsync();

        // Act
        var actualDescendants = entity.Descendants;

        // Assert
        actualDescendants
            .Should()
            .BeEquivalentTo(expectedDescendants);
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
    [InlineData("I", new[] { "J", "K", "L", "M", "N", "O" })]
    [InlineData("J", new[] { "K", "L" })]
    [InlineData("K", new[] { "L" })]
    [InlineData("L", new string[0])]
    [InlineData("M", new string[0])]
    [InlineData("N", new string[0])]
    [InlineData("O", new string[0])]
    public async Task DescendantsWithoutSelf_OfTestDataEntity_ShouldBeExpectedDescendants(
        string entityName,
        string[] expectedDescendantNames)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Descendants)
            .FirstAsync(e => e.Name == entityName);

        var expectedDescendants = await context
            .TestEntities
            .Where(e => expectedDescendantNames.Contains(e.Name))
            .ToListAsync();

        // Act
        var actualDescendants = entity.DescendantsWithoutSelf;

        // Assert
        actualDescendants
            .Should()
            .BeEquivalentTo(expectedDescendants);
    }
}
