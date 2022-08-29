using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectoryApi.Core.Events;

public class OpenReferralOrganisationCreatedEvent : DomainEventBase, IOpenReferralOrganisationCreatedEvent
{
    public OpenReferralOrganisationCreatedEvent(IOpenReferralOrganisation item)
    {
        Item = item;
    }

    public IOpenReferralOrganisation Item { get; }
}
