using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

public class TestEntity : SelfReferencingEntity<TestEntity, Guid>
{
    public TestEntity(TestEntity? parent) : base(parent)
    {
    }

    // ReSharper disable once UnusedMember.Local
    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    private TestEntity()
    {
    }
}
