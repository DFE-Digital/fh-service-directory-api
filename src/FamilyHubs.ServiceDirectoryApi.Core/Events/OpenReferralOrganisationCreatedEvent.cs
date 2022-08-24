using FamilyHubs.ServiceDirectory.Shared.Entities;
using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Interfaces.Events;

namespace fh_service_directory_api.core.Events;

public class OpenReferralOrganisationCreatedEvent : DomainEventBase, IOpenReferralOrganisationCreatedEvent
{
    public OpenReferralOrganisationCreatedEvent(IOpenReferralOrganisation item)
    {
        Item = item;
    }

    public IOpenReferralOrganisation Item { get; }
}
