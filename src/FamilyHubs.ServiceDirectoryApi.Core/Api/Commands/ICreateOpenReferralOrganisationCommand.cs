using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;

namespace FamilyHubs.ServiceDirectoryApi.Core.Api.Commands
{
    public interface ICreateOpenReferralOrganisationCommand
    {
        IOpenReferralOrganisationWithServicesDto OpenReferralOrganisation { get; init; }
    }
}