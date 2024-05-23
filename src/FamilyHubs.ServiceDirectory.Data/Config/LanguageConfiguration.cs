using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(100);

        // we store 2 char ISO 639-1 language codes, but the standard allows for 3 char ISO 639-3 codes
        builder.Property(t => t.Code)
            .HasMaxLength(3);

        builder.Property(t => t.Note)
            .HasMaxLength(512);

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