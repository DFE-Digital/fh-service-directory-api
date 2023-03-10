using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class FundingConfiguration : IEntityTypeConfiguration<Funding>
{
    public void Configure(EntityTypeBuilder<Funding> builder)
    {
        builder.Property(t => t.Created)
            .IsRequired();
        
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}