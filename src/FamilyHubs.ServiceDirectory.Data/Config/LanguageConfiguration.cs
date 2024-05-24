using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class LanguageConfiguration : EntityBaseConfiguration<Language>
{
    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name)
            .HasMaxLength(100);

        // we store 2 char ISO 639-1 language codes, but the standard allows for 3 char ISO 639-3 codes
        builder.Property(t => t.Code)
            .HasMaxLength(3);

        builder.Property(t => t.Note)
            .HasMaxLength(512);
    }
}