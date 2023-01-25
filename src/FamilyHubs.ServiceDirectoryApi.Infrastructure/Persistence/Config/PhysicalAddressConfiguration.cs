﻿using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;


public class PhysicalAddressConfiguration : IEntityTypeConfiguration<OpenReferralPhysical_Address>
{
    public void Configure(EntityTypeBuilder<OpenReferralPhysical_Address> builder)
    {
        builder.Property(t => t.Address_1)
            .IsRequired();
        builder.Property(t => t.Postal_code)
            .HasMaxLength(15)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
