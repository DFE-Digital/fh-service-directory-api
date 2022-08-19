using fh_service_directory_api.core.Concretions.Entities;
using fh_service_directory_api.core.Interfaces.Entities.Aggregates;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IServiceTaxonomy
    {
        string? LinkId { get; init; }

        ITaxonomy? Taxonomy { get; init; }
    }
}