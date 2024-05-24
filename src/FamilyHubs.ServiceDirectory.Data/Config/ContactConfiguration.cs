using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ContactConfiguration : EntityBaseConfiguration<Contact>
{
    public override void Configure(EntityTypeBuilder<Contact> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.ServiceId)
            .IsRequired(false);

        builder.Property(t => t.LocationId)
            .IsRequired(false);

        builder.Property(t => t.Title)
            .HasMaxLength(50);

        builder.Property(t => t.Name)
            .HasMaxLength(50);

        builder.Property(t => t.Telephone)
            .HasMaxLength(50);

        builder.Property(t => t.TextPhone)
            .HasMaxLength(50);

        builder.Property(t => t.Url)
            .HasMaxLength(2083);
    }
}