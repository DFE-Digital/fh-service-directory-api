using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config;

public class ServiceAtLocationConfiguration : IEntityTypeConfiguration<OpenReferralServiceAtLocation>
{
    public void Configure(EntityTypeBuilder<OpenReferralServiceAtLocation> builder)
    {
        //Needs investigating
        //builder.Property(t => t.Location)
        //    .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}
