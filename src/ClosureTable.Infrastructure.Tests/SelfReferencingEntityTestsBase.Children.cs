using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public abstract partial class SelfReferencingEntityTestsBase<TFixture>
{
    [Fact]
    public void Children_OfNewEntity_ShouldBeEmpty()
    {
        // Arrange
        static TestEntity Act() => new(null, "foo");

        // Act
        var newEntity = Act();

        // Assert
        newEntity.Children.Should().BeEmpty();
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
    [InlineData("I", new[] { "J", "M", "N", "O" })]
    [InlineData("J", new[] { "K" })]
    [InlineData("K", new[] { "L" })]
    [InlineData("L", new string[0])]
    [InlineData("M", new string[0])]
    [InlineData("N", new string[0])]
    [InlineData("O", new string[0])]
    public async Task Children_OfTestDataEntity_ShouldBeExpectedChildren(string entityName, string[] expectedChildNames)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Children)
            .FirstAsync(e => e.Name == entityName);

        var expectedChildren = await context
            .TestEntities
            .Where(e => expectedChildNames.Contains(e.Name))
            .ToListAsync();

        // Act
        var actualChildren = entity.Children;

        // Assert
        actualChildren
            .Should()
            .BeEquivalentTo(expectedChildren);
    }
}
