using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class AdminAreaConfiguration : IEntityTypeConfiguration<AdminArea>
{
    public void Configure(EntityTypeBuilder<AdminArea> builder)
    {
        builder.Property(t => t.Code)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.OrganisationId)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
