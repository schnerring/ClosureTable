using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

public class TestRelationship : AncestorDescendantRelationship<TestEntity, Guid, TestRelationship>
{
    public int Weight { get; }

    public TestRelationship(
        TestEntity ancestor,
        TestEntity descendant,
        int depth,
        int weight) : base(ancestor, descendant, depth)
    {
        Weight = weight;
    }

    // ReSharper disable once UnusedMember.Local
    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    private TestRelationship()
    {
        Weight = default!;
    }
}
