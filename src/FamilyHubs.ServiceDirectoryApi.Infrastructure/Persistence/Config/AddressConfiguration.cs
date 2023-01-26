using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class AddressConfiguration : IEntityTypeConfiguration<PhysicalAddress>
{
    public void Configure(EntityTypeBuilder<PhysicalAddress> builder)
    {
        builder.Property(t => t.Address1)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.City)
            .HasMaxLength(50);
        builder.Property(t => t.PostCode)
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
