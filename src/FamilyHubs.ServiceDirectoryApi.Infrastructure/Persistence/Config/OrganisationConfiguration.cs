using fh_service_directory_api.core.OrganisationAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config
{
    public class OrOrganisationConfiguration : IEntityTypeConfiguration<Organisation>
    {
        public void Configure(EntityTypeBuilder<Organisation> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedNever();
        }
    }
}
