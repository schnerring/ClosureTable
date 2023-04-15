using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

public class TestRelationshipProperties
{
    public int Weight { get; }

    public TestRelationshipProperties(int weight)
    {
        Weight = weight;
    }
}

public class TestRelationship : AncestorDescendantRelationship<TestEntity, Guid, TestRelationship, TestRelationshipProperties>
{
    public TestRelationship(
        TestEntity ancestor,
        TestEntity descendant,
        int depth,
        TestRelationshipProperties properties) : base(ancestor, descendant, depth, properties)
    {
    }

    // ReSharper disable once UnusedMember.Local
    // Required for EF constructor binding. See: https://github.com/dotnet/efcore/issues/12078
    private TestRelationship()
    {
    }
}
