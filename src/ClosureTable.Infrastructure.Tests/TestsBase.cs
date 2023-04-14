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
    public async Task Roots_OfTestData_ShouldHaveCountOf9()
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        // Act
        var roots = await context
            .TestEntities
            .Roots<TestEntity, Guid>()
            .ToListAsync();

        // Assert
        roots.Should().HaveCount(9);
    }

    [Fact]
    public async Task Roots_OfTestData_ShouldBeExpectedEntities()
    {
        // Arrange
        await using var context = _fixture.CreateContext();

        // Act
        var roots = await context
            .TestEntities
            .Roots<TestEntity, Guid>()
            .ToListAsync();

        // Assert
        roots
            .Select(e => e.Name)
            .Should()
            .BeEquivalentTo("A", "B", "C", "D", "E", "F", "G", "H", "I");
    }
}
