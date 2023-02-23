using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class RelatedOrganisationConfiguration : IEntityTypeConfiguration<RelatedOrganisation>
{
    public void Configure(EntityTypeBuilder<RelatedOrganisation> builder)
    {
        builder.Property(t => t.OrganisationId)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.RelatedOrganisationId)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}