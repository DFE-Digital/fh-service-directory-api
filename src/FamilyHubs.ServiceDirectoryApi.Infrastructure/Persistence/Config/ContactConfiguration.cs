using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class ContactConfiguration : IEntityTypeConfiguration<OpenReferralContact>
{
    public void Configure(EntityTypeBuilder<OpenReferralContact> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(50);
        builder.Property(t => t.Name)
            .HasMaxLength(50);
        builder.Property(t => t.Telephone)
            .HasMaxLength(50);
        builder.Property(t => t.TextPhone)
            .HasMaxLength(50);
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}