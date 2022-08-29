using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Config
{
    public class OpenReferralOpenReferralOrganisationConfiguration : IEntityTypeConfiguration<OpenReferralOrganisation>
    {
        public void Configure(EntityTypeBuilder<OpenReferralOrganisation> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedNever();
        }
    }
}
