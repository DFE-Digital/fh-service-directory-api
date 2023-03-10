using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class CostOptionConfiguration : IEntityTypeConfiguration<CostOption>
{
    public void Configure(EntityTypeBuilder<CostOption> builder)
    {
        builder.Property(t => t.AmountDescription)
            .HasMaxLength(500);

        builder.Property(t => t.Created)
            .IsRequired();
        
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(p => p.Taxonomies)
            .WithMany()
            .UsingEntity<LinkTaxonomy>("linktaxonomies",
                lt => lt
                    .HasOne<Taxonomy>()
                    .WithMany()
                    .HasForeignKey(c => c.TaxonomyId),
                rt => rt
                    .HasOne<CostOption>()
                    .WithMany()
                    .HasForeignKey(l => l.LinkId));
    }
}
