using fh_service_directory_api.core.Interfaces.Entities.Aggregates;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IEligibility
    {
        //string Eligibility { get; init; }

        string? LinkId { get; init; }

        int Maximum_age { get; init; }

        int Minimum_age { get; init; }

        ICollection<ITaxonomy>? Taxonomys { get; init; }
    }
}