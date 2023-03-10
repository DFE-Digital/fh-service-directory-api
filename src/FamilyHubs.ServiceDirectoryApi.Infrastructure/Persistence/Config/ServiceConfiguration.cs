using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.HasEnum(s => s.AttendingAccess);
        builder.HasEnum(s => s.AttendingType);
        builder.HasEnum(s => s.DeliverableType);
        builder.HasEnum(s => s.ServiceType);
        builder.HasEnum(s => s.Status);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(s => s.HolidaySchedules)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.RegularSchedules)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

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

        builder.HasMany(s => s.Reviews)
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

        builder.HasMany(p => p.Locations)
            .WithMany()
            .UsingEntity<ServiceAtLocation>(
                "serviceatlocations",
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
            .UsingEntity<ServiceTaxonomy>(
                "servicetaxonomies",
                lt => lt
                    .HasOne<Taxonomy>()
                    .WithMany()
                    .HasForeignKey(s => s.TaxonomyId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Service>()
                    .WithMany()
                    .HasForeignKey(s => s.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade)
                );

        builder.HasMany(p => p.Contacts)
            .WithMany()
            .UsingEntity<LinkContact>(
                "linkcontacts",
                lt => lt
                    .HasOne<Contact>()
                    .WithMany()
                    .HasForeignKey(c => c.ContactId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Service>()
                    .WithMany()
                    .HasForeignKey(l => l.LinkId)
                    .OnDelete(DeleteBehavior.Cascade),
                jt => jt
                    .HasEnum(t => t.LinkType)
                );
    }
}