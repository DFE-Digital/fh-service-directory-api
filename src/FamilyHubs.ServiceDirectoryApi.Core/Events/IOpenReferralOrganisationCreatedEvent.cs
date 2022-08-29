using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;

namespace FamilyHubs.ServiceDirectoryApi.Core.Events
{
    public interface IOpenReferralOrganisationCreatedEvent
    {
        IOpenReferralOrganisation Item { get; }
    }
}