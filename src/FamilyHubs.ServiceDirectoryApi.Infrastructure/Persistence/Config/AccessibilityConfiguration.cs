using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;
public class AccessibilityConfiguration : IEntityTypeConfiguration<AccessibilityForDisabilities>
{
    public void Configure(EntityTypeBuilder<AccessibilityForDisabilities> builder)
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