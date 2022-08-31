using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;
public class AccessibilityConfiguration : IEntityTypeConfiguration<Accessibility_For_Disabilities>
{
    public void Configure(EntityTypeBuilder<Accessibility_For_Disabilities> builder)
    {
        builder.Property(t => t.Accessibility)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

    }
}