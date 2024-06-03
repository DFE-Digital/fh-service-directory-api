using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ServiceDeliveryConfiguration : EntityBaseConfiguration<ServiceDelivery>
{
    public override void Configure(EntityTypeBuilder<ServiceDelivery> builder)
    {
        base.Configure(builder);

        builder.HasEnumProperty(t => t.Name, 50);
    }
}