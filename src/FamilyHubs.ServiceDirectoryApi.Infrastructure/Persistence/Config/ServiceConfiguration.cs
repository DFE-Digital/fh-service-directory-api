using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(s => s.LinkContacts)
            .WithOne()
            .HasForeignKey(lc => lc.LinkId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

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

        builder.HasMany(s => s.ServiceAtLocations)
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

        builder.HasMany(s => s.ServiceTaxonomies)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;
    }
}