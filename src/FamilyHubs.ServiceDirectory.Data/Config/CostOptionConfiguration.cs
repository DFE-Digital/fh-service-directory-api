﻿using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class CostOptionConfiguration : IEntityTypeConfiguration<CostOption>
{
    public void Configure(EntityTypeBuilder<CostOption> builder)
    {
        builder.Property(t => t.AmountDescription)
            .HasMaxLength(500);

        builder.Property(t => t.Option)
            .HasMaxLength(20);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);

        builder.Property(t => t.Currency)
            .HasMaxLength(3);
    }
}
