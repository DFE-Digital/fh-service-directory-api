using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class LinkTaxonomyConfiguration : IEntityTypeConfiguration<LinkTaxonomy>
{
    public void Configure(EntityTypeBuilder<LinkTaxonomy> builder)
    {
        builder.Property(t => t.LinkType)
            .HasMaxLength(50);
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}