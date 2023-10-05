using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class EligibilityConfiguration : IEntityTypeConfiguration<Eligibility>
{
    public void Configure(EntityTypeBuilder<Eligibility> builder)
    {
        builder.HasEnum(t => t.EligibilityType);

        builder.Property(t => t.EligibilityType)
                .HasMaxLength(50);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(320);
    }
}
