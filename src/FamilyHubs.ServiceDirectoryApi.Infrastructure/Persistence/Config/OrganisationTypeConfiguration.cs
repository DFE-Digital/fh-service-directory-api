using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class OrganisationTypeConfiguration : IEntityTypeConfiguration<OrganisationType>
{
    public void Configure(EntityTypeBuilder<OrganisationType> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(t => t.Description)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
