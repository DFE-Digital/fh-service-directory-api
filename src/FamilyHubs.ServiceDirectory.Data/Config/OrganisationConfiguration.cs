using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;


public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.Navigation(e => e.Services).AutoInclude();
        builder.Navigation(e => e.Locations).AutoInclude();

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
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsRequired()
            .IsEncrypted();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsEncrypted();

        builder.HasMany(s => s.Services)
            .WithOne()
            .HasForeignKey(s => s.OrganisationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        //todo: we'll have to revisit all the OnDelete behaviours
        builder.HasMany(s => s.Locations)
            .WithOne()
            .HasForeignKey(l => l.OrganisationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
