using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class CostOptionConfiguration : EntityBaseConfiguration<CostOption>
{
    public override void Configure(EntityTypeBuilder<CostOption> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.AmountDescription)
            .HasMaxLength(500);

        builder.Property(t => t.Option)
            .HasMaxLength(20);

        builder.Property(t => t.Currency)
            .HasMaxLength(3);
    }
}
