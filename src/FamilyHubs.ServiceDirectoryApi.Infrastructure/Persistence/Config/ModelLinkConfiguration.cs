using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class ModelLinkConfiguration : IEntityTypeConfiguration<ModelLink>
{
    public void Configure(EntityTypeBuilder<ModelLink> builder)
    {
        builder.Property(t => t.LinkType)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(t => t.ModelOneId)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.ModelOneId)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
