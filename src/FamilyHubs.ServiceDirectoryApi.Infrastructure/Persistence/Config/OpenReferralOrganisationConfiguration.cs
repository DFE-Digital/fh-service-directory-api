using FamilyHubs.ServiceDirectory.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fh_service_directory_api.infrastructure.Persistence.Config
{
    public class OpenReferralOpenReferralOrganisationConfiguration : IEntityTypeConfiguration<OpenReferralOrganisation>
    {
        public void Configure(EntityTypeBuilder<OpenReferralOrganisation> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedNever();
        }
    }
}
