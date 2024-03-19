using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Navigation(e => e.Contacts).AutoInclude();
        builder.Navigation(e => e.Schedules).AutoInclude();

        builder.HasEnumProperty(t => t.LocationTypeCategory);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Address1)
            .HasMaxLength(100);

        builder.Property(t => t.Address2)
            .HasMaxLength(100);

        builder.Property(t => t.City)
            .HasMaxLength(60);

        builder.Property(t => t.StateProvince)
            .HasMaxLength(60);

        builder.Property(t => t.Country)
            .HasMaxLength(60);

        builder.Property(t => t.PostCode)
            .HasMaxLength(15);

        builder.HasEnumProperty(t => t.LocationType);

        builder.Property(t => t.AddressType)
            .HasMaxLength(10);

        builder.Property(t => t.AlternateName)
            .HasMaxLength(255);

        builder.Property(t => t.Attention)
            .HasMaxLength(255);

        builder.Property(t => t.Region)
            .HasMaxLength(255);

        builder.Property(t => t.Transportation)
            .HasMaxLength(500);

        builder.Property(t => t.Url)
            .HasMaxLength(2083);

        builder.Property(t => t.ExternalIdentifier)
            .HasMaxLength(500);

        builder.Property(t => t.ExternalIdentifierType)
            .HasMaxLength(500);

        builder.HasMany(s => s.AccessibilityForDisabilities)
            .WithOne()
            .HasForeignKey(lc => lc.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Schedules)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(lc => lc.LocationId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Contacts)
            .WithOne()
            .HasForeignKey(lc => lc.LocationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}