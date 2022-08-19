using fh_service_directory_api.core.OrganisationAggregate.Entities;
using LocalAuthorityInformationServices.SharedKernel;

namespace fh_service_directory_api.core.OrganisationAggregate.Events;

public class NewServiceAddedEvent : DomainEventBase
{
    public NewServiceAddedEvent(IOrganisation organisation, IService newServiceAdded)
    {
        Organisation = organisation;
        NewServiceAdded = newServiceAdded;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public IService NewServiceAdded { get; private set; }

    public IOrganisation Organisation { get; private set; }
}