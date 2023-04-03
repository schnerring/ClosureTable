using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

public class TestEntity : SelfReferencingEntity<TestEntity, Guid>
{
    public string Name { get; }

    public TestEntity(TestEntity? parent, string name) : base(parent)
    {
        Name = name;
    }

    // ReSharper disable once UnusedMember.Local
    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    private TestEntity()
    {
        Name = default!;
    }
}
