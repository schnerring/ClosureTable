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
}
