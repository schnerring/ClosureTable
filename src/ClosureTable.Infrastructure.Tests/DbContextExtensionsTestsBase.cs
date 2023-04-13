namespace ClosureTable.Infrastructure.Tests;

public abstract partial class DbContextExtensionsTestsBase<TFixture> : IClassFixture<TFixture>, IAsyncLifetime
    where TFixture : DatabaseFixtureBase
{
    private readonly TFixture _fixture;

    protected DbContextExtensionsTestsBase(TFixture fixture)
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
}
