using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;


public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasEnum(t => t.OrganisationType);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(s => s.Reviews)
            .WithOne()
            .HasForeignKey(lc => lc.OrganisationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(p => p.Taxonomies)
            .WithMany()
            .UsingEntity<LinkTaxonomy>(
                "linktaxonomies",
                lt => lt
                    .HasOne<Taxonomy>()
                    .WithMany()
                    .HasForeignKey(c => c.TaxonomyId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Organisation>()
                    .WithMany()
                    .HasForeignKey(l => l.LinkId)
                    .OnDelete(DeleteBehavior.Cascade),
                jt => jt
                    .HasEnum(t => t.LinkType)
            );


        builder.HasMany(p => p.Contacts)
            .WithMany()
            .UsingEntity<LinkContact>(
                "linkcontacts",
                lt => lt
                    .HasOne<Contact>()
                    .WithMany()
                    .HasForeignKey(c => c.ContactId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Organisation>()
                    .WithMany()
                    .HasForeignKey(l => l.LinkId)
                    .OnDelete(DeleteBehavior.Cascade),
                jt => jt.
                    HasEnum(t => t.LinkType)
            );
    }
}
