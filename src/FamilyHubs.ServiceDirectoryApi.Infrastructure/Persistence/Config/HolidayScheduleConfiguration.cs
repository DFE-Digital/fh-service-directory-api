﻿using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class HolidayScheduleConfiguration : IEntityTypeConfiguration<HolidaySchedule>
{
    public void Configure(EntityTypeBuilder<HolidaySchedule> builder)
    {
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