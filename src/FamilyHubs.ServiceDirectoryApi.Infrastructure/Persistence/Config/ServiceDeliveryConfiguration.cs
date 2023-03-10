using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class ServiceDeliveryConfiguration : IEntityTypeConfiguration<ServiceDelivery>
{
    public void Configure(EntityTypeBuilder<ServiceDelivery> builder)
    {
        builder.HasEnum(t => t.Name);

        builder.Property(t => t.Name)
            .HasMaxLength(50);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}