﻿using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class TaxonomyConfiguration : IEntityTypeConfiguration<Taxonomy>
{
    public void Configure(EntityTypeBuilder<Taxonomy> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.HasEnum(t => t.TaxonomyType);

        builder.Property(t => t.TaxonomyType)
                .HasMaxLength(50);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(320);
    }
}