using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ServiceAtLocationConfiguration : IEntityTypeConfiguration<ServiceAtLocation>
{
    public void Configure(EntityTypeBuilder<ServiceAtLocation> builder)
    {
        builder.ToTable("ServiceAtLocations");

        builder.Navigation(e => e.Schedules).AutoInclude();

        builder.HasMany(s => s.Schedules)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceAtLocationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction)
            ;

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}