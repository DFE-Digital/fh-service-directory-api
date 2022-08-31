using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class ServiceDeliveryConfiguration : IEntityTypeConfiguration<OpenReferralServiceDelivery>
{
    public void Configure(EntityTypeBuilder<OpenReferralServiceDelivery> builder)
    {
        builder.Property(t => t.ServiceDelivery)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}