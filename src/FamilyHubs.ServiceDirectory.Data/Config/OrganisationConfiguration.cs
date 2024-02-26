using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.Navigation(e => e.Location).AutoInclude();

        //todo: probably don't want to AutoInclude, but would require some refactoring
        //org dto doesn't have a services collection, so think ok without AutoInclude() : check
        builder.Navigation(e => e.Services).AutoInclude();
        
        builder.HasEnumProperty(t => t.OrganisationType, 50);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.AdminAreaCode)
            .HasMaxLength(15);

        builder.Property(t => t.Logo)
            .HasMaxLength(2083);

        builder.Property(t => t.Url)
            .HasMaxLength(2083);

        builder.Property(t => t.Uri)
            .HasMaxLength(2083);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);

        //builder.HasMany(s => s.Services)
        //    .WithOne()
        //    .IsRequired(false)
        //    .HasForeignKey(lc => lc.OrganisationId)
        //    .OnDelete(DeleteBehavior.ClientNoAction)
        //    ;

        builder.HasMany(s => s.Location)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(lc => lc.OrganisationId)
            .OnDelete(DeleteBehavior.ClientNoAction)
            ;


        //        migrationBuilder.Sql(
        //            @"UPDATE Locations
        //SET OrganisationId = Services.OrganisationId
        //FROM Locations
        //INNER JOIN ServiceAtLocations ON Locations.Id = ServiceAtLocations.LocationId
        //INNER JOIN Services ON ServiceAtLocations.ServiceId = Services.Id
        //WHERE Locations.OrganisationId = 0;");

    }
}
