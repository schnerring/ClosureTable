using System.Diagnostics;
using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

[DebuggerDisplay("{Name}")]
public class TestEntity : SelfReferencingEntity<TestEntity, Guid, TestRelationship>, IRelationshipFactory<TestEntity, Guid, TestRelationship>
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

    public static TestRelationship CreateRelationship(
        TestEntity ancestor,
        TestEntity descendant,
        int depth,
        IRelationshipOptions options)
    {
        return new TestRelationship(ancestor, descendant, depth, 0); // TODO
    }
}
