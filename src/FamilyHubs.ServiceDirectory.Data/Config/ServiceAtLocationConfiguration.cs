using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FamilyHubs.ServiceDirectory.Data.Entities;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ServiceAtLocationConfiguration : IEntityTypeConfiguration<ServiceAtLocation>
{
    public void Configure(EntityTypeBuilder<ServiceAtLocation> builder)
    {
        builder.ToTable("ServiceAtLocations");

        //builder.HasKey(sal => new { sal.ServiceId, sal.LocationId });

        builder.HasMany(s => s.Schedules)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(lc => lc.ServiceAtLocationId)
            .OnDelete(DeleteBehavior.NoAction)
            ;

        //builder.HasOne(sal => sal.Schedule)
        //    .WithOne(s => s.ServiceAtLocation)
        //    .HasForeignKey<ServiceAtLocation>(sal => sal.ScheduleId)
        //    .OnDelete(DeleteBehavior.NoAction);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}