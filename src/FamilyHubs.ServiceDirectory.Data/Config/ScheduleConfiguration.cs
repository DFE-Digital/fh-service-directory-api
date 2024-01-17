using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasEnum(t => t.Freq);

        builder.Property(t => t.Freq)
                .HasMaxLength(50);

        builder.Property(t => t.DtStart)
            .HasMaxLength(30);

        builder.Property(t => t.Interval)
            .HasMaxLength(30);

        builder.Property(t => t.ByDay)
            .HasMaxLength(15);

        builder.Property(t => t.ByMonthDay)
            .HasMaxLength(15);

        builder.Property(t => t.Weekday)
            .IsRequired(false);

        builder.Property(t => t.ServiceId)
            .IsRequired(false);

        builder.Property(t => t.LocationId)
            .IsRequired(false);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}
