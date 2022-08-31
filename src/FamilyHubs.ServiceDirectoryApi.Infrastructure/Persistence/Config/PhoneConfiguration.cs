using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class PhoneConfiguration : IEntityTypeConfiguration<OpenReferralPhone>
{
    public void Configure(EntityTypeBuilder<OpenReferralPhone> builder)
    {
        builder.Property(t => t.Number)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }

}
