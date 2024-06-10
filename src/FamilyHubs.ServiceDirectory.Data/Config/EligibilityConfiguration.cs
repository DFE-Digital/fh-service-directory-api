using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class EligibilityConfiguration : EntityBaseConfiguration<Eligibility>
{
    public override void Configure(EntityTypeBuilder<Eligibility> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.EligibilityType)
            .HasMaxLength(50)
            .HasConversion<string>();
    }
}
