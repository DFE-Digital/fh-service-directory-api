using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
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

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsRequired()
            .IsEncrypted();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsEncrypted();
    }
}
