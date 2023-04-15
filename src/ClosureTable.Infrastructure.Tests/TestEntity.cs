using System.Diagnostics;
using ClosureTable.Models;

namespace ClosureTable.Infrastructure.Tests;

[DebuggerDisplay("{Name}")]
public class TestEntity : SelfReferencingEntity<TestEntity, Guid, TestRelationship, TestRelationshipProperties>, IRelationshipFactory<TestEntity, Guid, TestRelationship, TestRelationshipProperties>
{
    public string Name { get; }

    public TestEntity(TestEntity? parent, string name, TestRelationshipProperties properties) : base(parent, properties)
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
        TestRelationshipProperties properties)
    {
        return new TestRelationship(
            ancestor,
            descendant,
            depth,
            properties);
    }
}
