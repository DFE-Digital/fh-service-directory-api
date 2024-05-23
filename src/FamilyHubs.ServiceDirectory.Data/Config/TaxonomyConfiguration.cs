using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class TaxonomyConfiguration : EntityBaseConfiguration<Taxonomy>
{
    public override void Configure(EntityTypeBuilder<Taxonomy> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.HasEnumProperty(t => t.TaxonomyType, 50);
    }
}