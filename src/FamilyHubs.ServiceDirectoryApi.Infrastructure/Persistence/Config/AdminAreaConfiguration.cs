using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class AdminAreaConfiguration : IEntityTypeConfiguration<AdminArea>
{
    public void Configure(EntityTypeBuilder<AdminArea> builder)
    {
        builder.Property(t => t.Code)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.OpenReferralOrganisationId)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
