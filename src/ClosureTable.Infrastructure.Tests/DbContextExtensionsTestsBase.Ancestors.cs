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
}
