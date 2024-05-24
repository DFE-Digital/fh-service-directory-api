using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class FundingConfiguration : EntityBaseConfiguration<Funding>
{
    public override void Configure(EntityTypeBuilder<Funding> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Source)
            .HasMaxLength(255);
    }
}