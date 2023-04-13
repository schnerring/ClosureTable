using FluentAssertions;

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
}
