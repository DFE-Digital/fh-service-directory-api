using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class ServiceAreaConfiguration : IEntityTypeConfiguration<OpenReferralService_Area>
{
    public void Configure(EntityTypeBuilder<OpenReferralService_Area> builder)
    {
        builder.Property(t => t.Service_area)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
