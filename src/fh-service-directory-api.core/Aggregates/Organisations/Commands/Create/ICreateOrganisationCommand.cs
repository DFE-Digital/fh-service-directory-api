using fh_service_directory_api.core.Aggregates.Organisations.Entities;

namespace fh_service_directory_api.core.Aggregates.Organisations.Commands.Create
{
    public interface ICreateOrganisationCommand
    {
        IOrganisation Organisation { get; init; }
    }
}