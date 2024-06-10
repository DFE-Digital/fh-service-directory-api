using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ServiceAtLocationConfiguration : EntityBaseConfiguration<ServiceAtLocation>
{
    public override void Configure(EntityTypeBuilder<ServiceAtLocation> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceAtLocations");

        builder.Navigation(e => e.Schedules).AutoInclude();

        builder.HasMany(s => s.Schedules)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceAtLocationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction)
            ;
    }
}