using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class EligibilityConfiguration : IEntityTypeConfiguration<OpenReferralEligibility>
{
    public void Configure(EntityTypeBuilder<OpenReferralEligibility> builder)
    {
        builder.Property(t => t.Eligibility)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
