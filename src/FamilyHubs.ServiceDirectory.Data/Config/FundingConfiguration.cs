using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class FundingConfiguration : IEntityTypeConfiguration<Funding>
{
    public void Configure(EntityTypeBuilder<Funding> builder)
    {
        builder.Property(t => t.Source)
            .HasMaxLength(255);

        // have base configuration for these properties?
        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsRequired()
            .IsEncrypted();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsEncrypted();
    }
}