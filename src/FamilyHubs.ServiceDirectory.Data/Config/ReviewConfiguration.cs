using FamilyHubs.ServiceDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(t => t.Score)
            .HasMaxLength(10);

        builder.Property(t => t.Url)
            .HasMaxLength(2083);

        builder.Property(t => t.Widget)
            .HasMaxLength(2083);

        builder.Property(t => t.ServiceId)
            .IsRequired(false);

        builder.Property(t => t.OrganisationId)
            .IsRequired(false);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}