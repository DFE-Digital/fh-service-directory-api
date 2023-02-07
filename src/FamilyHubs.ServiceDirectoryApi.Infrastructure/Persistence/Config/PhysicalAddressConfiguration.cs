using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;


public class PhysicalAddressConfiguration : IEntityTypeConfiguration<PhysicalAddress>
{
    public void Configure(EntityTypeBuilder<PhysicalAddress> builder)
    {
        builder.Property(t => t.Address1)
            .IsRequired();
        builder.Property(t => t.PostCode)
            .HasMaxLength(15)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
