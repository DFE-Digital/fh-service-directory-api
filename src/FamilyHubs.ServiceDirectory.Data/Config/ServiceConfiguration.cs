using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Navigation(e => e.ServiceDeliveries).AutoInclude();
        builder.Navigation(e => e.CostOptions).AutoInclude();
        builder.Navigation(e => e.Eligibilities).AutoInclude();
        builder.Navigation(e => e.Fundings).AutoInclude();
        builder.Navigation(e => e.Languages).AutoInclude();
        builder.Navigation(e => e.Reviews).AutoInclude();
        builder.Navigation(e => e.ServiceAreas).AutoInclude();
        builder.Navigation(e => e.Taxonomies).AutoInclude();
        builder.Navigation(e => e.Locations).AutoInclude();
        builder.Navigation(e => e.Contacts).AutoInclude();
        builder.Navigation(e => e.HolidaySchedules).AutoInclude();
        builder.Navigation(e => e.RegularSchedules).AutoInclude();

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.HasEnum(s => s.AttendingAccess);
        builder.Property(t => t.AttendingAccess)
                .HasMaxLength(50);
        builder.HasEnum(s => s.AttendingType);
        builder.Property(t => t.AttendingType)
                .HasMaxLength(50);
        builder.HasEnum(s => s.DeliverableType);
        builder.Property(t => t.DeliverableType)
                .HasMaxLength(50);
        builder.HasEnum(s => s.ServiceType);
        builder.Property(t => t.ServiceType)
                .HasMaxLength(50);
        builder.HasEnum(s => s.Status);
        builder.Property(t => t.Status)
                .HasMaxLength(50);

        builder.Property(t => t.InterpretationServices)
            .HasMaxLength(512);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);

        builder.HasMany(s => s.Fundings)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.CostOptions)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Eligibilities)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Languages)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.ServiceAreas)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.ServiceDeliveries)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Contacts)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.HolidaySchedules)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(lc => lc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.RegularSchedules)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(lc => lc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Reviews)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(p => p.Locations)
            .WithMany()
            .UsingEntity<ServiceAtLocation>("ServiceAtLocations",
                lt => lt
                    .HasOne<Location>()
                    .WithMany()
                    .HasForeignKey(s => s.LocationId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Service>()
                    .WithMany()
                    .HasForeignKey(s => s.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade));

        builder.HasMany(p => p.Taxonomies)
            .WithMany()
            .UsingEntity<ServiceTaxonomy>("ServiceTaxonomies",
                lt => lt
                    .HasOne<Taxonomy>()
                    .WithMany()
                    .HasForeignKey(s => s.TaxonomyId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Service>()
                    .WithMany()
                    .HasForeignKey(s => s.ServiceId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade)
                );
    }
}