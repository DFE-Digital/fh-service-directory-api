using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class EligibilityConfiguration : IEntityTypeConfiguration<Eligibility>
{
    public void Configure(EntityTypeBuilder<Eligibility> builder)
    {
        builder.Property(t => t.EligibilityType)
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}
