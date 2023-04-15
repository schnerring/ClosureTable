using ClosureTable.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClosureTable.Infrastructure.Configuration;

public abstract class SelfReferencingEntityConfiguration<TEntity, TKey, TRelationship, TRelationshipProperties> : IEntityTypeConfiguration<TEntity>
    where TEntity : SelfReferencingEntity<TEntity, TKey, TRelationship, TRelationshipProperties>, IRelationshipFactory<TEntity, TKey, TRelationship, TRelationshipProperties>
    where TKey : struct
    where TRelationship : AncestorDescendantRelationship<TEntity, TKey, TRelationship, TRelationshipProperties>
    where TRelationshipProperties : class
{
    private readonly Action<OwnedNavigationBuilder<TRelationship, TRelationshipProperties>> _propertiesBuilder;

    protected SelfReferencingEntityConfiguration(Action<OwnedNavigationBuilder<TRelationship, TRelationshipProperties>> propertiesBuilder)
    {
        _propertiesBuilder = propertiesBuilder;
    }

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id);

        // Adjacency List

        builder
            .HasOne(entity => entity.Parent)
            .WithMany(entity => entity.Children)
            .HasForeignKey(entity => entity.ParentId);

        builder
            .Navigation(entity => entity.Children)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Closure Table

        builder
            .HasMany(entity => entity.Ancestors)
            .WithMany(entity => entity.Descendants)
            .UsingEntity<TRelationship>(
                relationshipBuilder => relationshipBuilder
                    .HasOne(relationship => relationship.Ancestor)
                    .WithMany(entity => entity.DescendantRelationships)
                    .HasForeignKey(relationship => relationship.AncestorId)
                    .OnDelete(DeleteBehavior.Cascade),
                relationshipBuilder => relationshipBuilder
                    .HasOne(relationship => relationship.Descendant)
                    .WithMany(entity => entity.AncestorRelationships)
                    .HasForeignKey(relationship => relationship.DescendantId)
                    // SQL Server itself is unable to resolve circular cascades.
                    // We let EF take care of this.
                    .OnDelete(DeleteBehavior.ClientCascade),
                relationshipBuilder =>
                {
                    relationshipBuilder
                        .HasKey(relationship => new { relationship.AncestorId, relationship.DescendantId });
                    relationshipBuilder
                        .Navigation(relationship => relationship.Ancestor)
                        .UsePropertyAccessMode(PropertyAccessMode.Field);
                    relationshipBuilder
                        .Navigation(relationship => relationship.Descendant)
                        .UsePropertyAccessMode(PropertyAccessMode.Field);
                    relationshipBuilder.Property(relationship => relationship.Depth);
                    relationshipBuilder.OwnsOne(
                        relationship => relationship.Properties,
                        _propertiesBuilder);
                }
            )
            .ToTable(typeof(TEntity).Name);

        builder
            .Navigation(entity => entity.Ancestors)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .Navigation(entity => entity.AncestorRelationships)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .Navigation(entity => entity.Descendants)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .Navigation(entity => entity.DescendantRelationships)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
