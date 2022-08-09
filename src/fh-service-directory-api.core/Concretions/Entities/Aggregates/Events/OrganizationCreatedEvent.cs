using fh_service_directory_api.core.Interfaces.Entities.Aggregates;
using LocalAuthorityInformationServices.SharedKernel;

namespace fh_service_directory_api.core.Concretions.Entities.Aggregates.Events;

public class OrganisationCreated : DomainEventBase
{
    public IOrganisation Organisation { get; set; }

    public IReview Review { get; set; }

    public IService Service { get; set; }

    public OrganisationCreated
    (
        IOrganisation organisation,
        IReview review,
        IService service
    )
    {
        Organisation = organisation;
        Review = review;
        Service = service;
    }
}
