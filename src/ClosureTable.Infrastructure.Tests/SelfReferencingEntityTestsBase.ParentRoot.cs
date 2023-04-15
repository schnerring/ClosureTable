using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ClosureTable.Infrastructure.Tests;

public abstract partial class SelfReferencingEntityTestsBase<TFixture>
{
    [Fact]
    public void IsParent_OfNewEntity_ShouldBeFalse()
    {
        // Arrange
        static TestEntity Act() => new(null, "foo");

        // Act
        var newEntity = Act();

        // Assert
        newEntity.IsParent.Should().BeFalse();
    }

    [Fact]
    public void IsRoot_OfNewEntity_ShouldBeTrue()
    {
        // Arrange
        static TestEntity Act() => new(null, "foo");

        // Act
        var newEntity = Act();

        // Assert
        newEntity.IsRoot.Should().BeTrue();
    }

    [Theory]
    [InlineData("A", true)]
    [InlineData("B", true)]
    [InlineData("C", true)]
    [InlineData("D", true)]
    [InlineData("E", true)]
    [InlineData("F", true)]
    [InlineData("G", true)]
    [InlineData("H", true)]
    [InlineData("I", true)]
    [InlineData("J", false)]
    [InlineData("K", false)]
    [InlineData("L", false)]
    [InlineData("M", false)]
    [InlineData("N", false)]
    [InlineData("O", false)]
    public async Task IsRoot_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Parent)
            .AsNoTracking()
            .FirstAsync(e => e.Name == entityName);

        // Act
        var actual = entity.IsRoot;

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
    public async Task IsParent_OfTestDataEntity_ShouldBeExpected(string entityName, bool expected)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Children)
            .AsNoTracking()
            .FirstAsync(e => e.Name == entityName);

        // Act
        var actual = entity.IsParent;

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("A", null)]
    [InlineData("B", null)]
    [InlineData("C", null)]
    [InlineData("D", null)]
    [InlineData("E", null)]
    [InlineData("F", null)]
    [InlineData("G", null)]
    [InlineData("H", null)]
    [InlineData("I", null)]
    [InlineData("J", "I")]
    [InlineData("K", "J")]
    [InlineData("L", "K")]
    [InlineData("M", "I")]
    [InlineData("N", "I")]
    [InlineData("O", "I")]
    public async Task Parent_OfTestDataEntity_ShouldBeExpected(string entityName, string? expectedParentName)
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        var entity = await context
            .TestEntities
            .Include(e => e.Parent)
            .FirstAsync(e => e.Name == entityName);

        var expected = expectedParentName == null
            ? null
            : await context
                .TestEntities
                .FirstAsync(e => e.Name == expectedParentName);

        // Act
        var actual = entity.Parent;

        // Assert
        actual.Should().Be(expected);
    }
}
