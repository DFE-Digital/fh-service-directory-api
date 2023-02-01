﻿using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class UiCacheConfig : IEntityTypeConfiguration<UiCache>
{
    public void Configure(EntityTypeBuilder<UiCache> builder)
    {
        builder.Property(t => t.Id)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Value)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }

}