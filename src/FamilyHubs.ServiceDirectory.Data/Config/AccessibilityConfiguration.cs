using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class AccessibilityConfiguration : IEntityTypeConfiguration<AccessibilityForDisabilities>
{
    public void Configure(EntityTypeBuilder<AccessibilityForDisabilities> builder)
    {
        builder.Property(t => t.Accessibility)
            .HasMaxLength(255);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}