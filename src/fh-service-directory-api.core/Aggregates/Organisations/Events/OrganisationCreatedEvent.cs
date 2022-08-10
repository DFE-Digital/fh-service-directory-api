using fh_service_directory_api.core.Aggregates.Organisations.Entities;
using fh_service_directory_api.core.Aggregates.Services.Entities;
using fh_service_directory_api.core.Interfaces.Entities.Aggregates;
using LocalAuthorityInformationServices.SharedKernel;

namespace fh_service_directory_api.core.Concretions.Entities.Aggregates.Organisatons.Events;

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
