using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;


public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.Navigation(e => e.Reviews).AutoInclude();
        builder.Navigation(e => e.Services).AutoInclude();
        
        builder.HasEnum(t => t.OrganisationType);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(s => s.Reviews)
            .WithOne()
            .HasForeignKey(lc => lc.OrganisationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction)
            ;
    }
}
