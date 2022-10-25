using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OrganisationAdminDistrictCreatedEvent : DomainEventBase
{
    public OrganisationAdminDistrictCreatedEvent(OrganisationAdminDistrict item)
    {
        Item = item;
    }

    public OrganisationAdminDistrict Item { get; }
}

