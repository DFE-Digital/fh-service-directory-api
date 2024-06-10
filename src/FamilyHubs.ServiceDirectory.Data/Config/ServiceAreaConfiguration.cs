using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ServiceAreaConfiguration : EntityBaseConfiguration<ServiceArea>
{
    public override void Configure(EntityTypeBuilder<ServiceArea> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.ServiceAreaName)
            .HasMaxLength(255);

        builder.Property(t => t.Extent)
            .HasMaxLength(255);

        builder.Property(t => t.Uri)
            .HasMaxLength(2083);
    }
}
