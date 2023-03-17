using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class RegularScheduleConfiguration : IEntityTypeConfiguration<RegularSchedule>
{
    public void Configure(EntityTypeBuilder<RegularSchedule> builder)
    {
        builder.HasEnum(t => t.Freq);

        builder.HasEnum(t => t.Weekday);

        builder.Property(t => t.ServiceId)
            .IsRequired(false);

        builder.Property(t => t.LocationId)
            .IsRequired(false);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
