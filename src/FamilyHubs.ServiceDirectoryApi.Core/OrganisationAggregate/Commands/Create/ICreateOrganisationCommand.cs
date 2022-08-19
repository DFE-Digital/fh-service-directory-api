using fh_service_directory_api.core.OrganisationAggregate.Entities;

namespace fh_service_directory_api.core.OrganisationAggregate.Commands.Create
{
    public interface ICreateOrganisationCommand
    {
        IOrganisation Organisation { get; init; }
    }
}