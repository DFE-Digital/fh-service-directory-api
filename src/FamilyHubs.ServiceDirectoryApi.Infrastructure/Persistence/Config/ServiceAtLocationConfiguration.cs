using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Config;

public class ServiceAtLocationConfiguration : IEntityTypeConfiguration<ServiceAtLocation>
{
    public void Configure(EntityTypeBuilder<ServiceAtLocation> builder)
    {
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasMany(s => s.LinkContacts)
            .WithOne()
            .HasForeignKey(lc => lc.LinkId);
    }
}
