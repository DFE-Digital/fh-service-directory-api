using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
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
            .HasForeignKey(lc => lc.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.RegularSchedules)
            .WithOne()
            .HasForeignKey(lc => lc.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(p => p.Contacts)
            .WithMany()
            .UsingEntity<LinkContact>("linkcontacts",
                lt => lt
                    .HasOne<Contact>()
                    .WithMany()
                    .HasForeignKey(c => c.ContactId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Location>()
                    .WithMany()
                    .HasForeignKey(l => l.LinkId)
                    .OnDelete(DeleteBehavior.Cascade),
                jt => jt.
                    HasEnum(t => t.LinkType)
            );
    }
}