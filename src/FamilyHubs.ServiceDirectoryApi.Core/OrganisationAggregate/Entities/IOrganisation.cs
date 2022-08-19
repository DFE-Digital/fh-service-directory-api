using fh_service_directory_api.core.Interfaces.Entities.Aggregates;

namespace fh_service_directory_api.core.OrganisationAggregate.Entities
{
    public interface IOrganisation
    {
        string Description { get; }
        string? Logo { get; }
        string Name { get; }
        IEnumerable<IReview> Reviews { get; }
        IEnumerable<IService> Services { get; }
        string? Uri { get; }
        string? Url { get; }

        IService AddNewService(IService service);
        void UpdateOrganisation(string name, string description, string? logo = null, string? uri = null, string? url = null);
    }
}