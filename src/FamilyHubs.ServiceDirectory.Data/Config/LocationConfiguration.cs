using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Navigation(e => e.Contacts).AutoInclude();
        builder.Navigation(e => e.HolidaySchedules).AutoInclude();
        builder.Navigation(e => e.RegularSchedules).AutoInclude();
        
        builder.HasEnum(t => t.LocationType);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.City)
            .HasMaxLength(60);

        builder.Property(t => t.PostCode)
            .HasMaxLength(15);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(s => s.AccessibilityForDisabilities)
            .WithOne()
            .HasForeignKey(lc => lc.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.HolidaySchedules)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(lc => lc.LocationId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.RegularSchedules)
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
    }
}