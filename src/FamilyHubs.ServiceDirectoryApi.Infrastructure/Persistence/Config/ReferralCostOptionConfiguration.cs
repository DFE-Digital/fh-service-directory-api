using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class ReferralCostOptionConfiguration : IEntityTypeConfiguration<CostOption>
{
    public void Configure(EntityTypeBuilder<CostOption> builder)
    {
        builder.Property(t => t.Amount)
            .IsRequired();
        builder.Property(t => t.AmountDescription)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
