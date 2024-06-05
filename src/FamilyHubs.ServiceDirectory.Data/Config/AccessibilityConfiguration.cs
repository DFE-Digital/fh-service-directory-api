using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class AccessibilityConfiguration : EntityBaseConfiguration<AccessibilityForDisabilities>
{
    public override void Configure(EntityTypeBuilder<AccessibilityForDisabilities> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Accessibility)
            .HasMaxLength(255);
    }
}