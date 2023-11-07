﻿using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;


public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.Navigation(e => e.Reviews).AutoInclude();
        builder.Navigation(e => e.Services).AutoInclude();
        
        builder.HasEnum(t => t.OrganisationType);

        builder.Property(t => t.OrganisationType)
                .HasMaxLength(50);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.AdminAreaCode)
            .HasMaxLength(15);

        builder.Property(t => t.Logo)
            .HasMaxLength(2083);

        builder.Property(t => t.Url)
            .HasMaxLength(2083);

        builder.Property(t => t.Uri)
            .HasMaxLength(2083);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(320);

        builder.HasMany(s => s.Reviews)
            .WithOne()
            .HasForeignKey(lc => lc.OrganisationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction)
            ;
    }
}