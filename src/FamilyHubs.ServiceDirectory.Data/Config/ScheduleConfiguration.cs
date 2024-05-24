using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ScheduleConfiguration : EntityBaseConfiguration<Schedule>
{
    public override void Configure(EntityTypeBuilder<Schedule> builder)
    {
        base.Configure(builder);

        builder.HasEnumProperty(t => t.Freq)
            .IsUnicode(false);

        builder.Property(t => t.DtStart)
            .HasMaxLength(30)
            .IsUnicode(false);

        builder.Property(t => t.ByDay)
            .HasMaxLength(34)
            .IsUnicode(false);

        builder.Property(t => t.ByMonthDay)
            .HasMaxLength(15)
            .IsUnicode(false);

        builder.Property(t => t.Until)
            .HasMaxLength(300)
            .IsUnicode(false);

        builder.Property(t => t.WkSt)
            .HasMaxLength(300)
            .IsUnicode(false);

        builder.Property(t => t.ByWeekNo)
            .HasMaxLength(300)
            .IsUnicode(false);

        builder.Property(t => t.ByYearDay)
            .HasMaxLength(300)
            .IsUnicode(false);

        builder.Property(t => t.ScheduleLink)
            .HasMaxLength(600)
            .IsUnicode(false);
    }
}
