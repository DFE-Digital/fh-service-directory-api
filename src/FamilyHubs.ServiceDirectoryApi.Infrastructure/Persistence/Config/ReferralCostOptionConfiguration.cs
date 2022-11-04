using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class ReferralCostOptionConfiguration : IEntityTypeConfiguration<OpenReferralCost_Option>
{
    public void Configure(EntityTypeBuilder<OpenReferralCost_Option> builder)
    {
        builder.Property(t => t.Amount)
            .IsRequired();
        builder.Property(t => t.Amount_description)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
