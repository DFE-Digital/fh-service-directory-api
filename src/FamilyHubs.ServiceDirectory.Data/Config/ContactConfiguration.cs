using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
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
            .HasMaxLength(320);

        builder.Property(t => t.Url)
            .HasMaxLength(2083);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}