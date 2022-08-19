using fh_service_directory_api.core.Concretions.Entities;
using fh_service_directory_api.core.Concretions.Entities.Aggregates;

namespace fh_service_directory_api.core.Interfaces.Entities.Aggregates
{
    public interface ITaxonomy
    {
        ICollection<LinkTaxonomyCollection>? LinkTaxonomyCollection { get; init; }
        string Name { get; init; }
        string? Parent { get; init; }
        ICollection<Taxonomy>? ServiceTaxonomyCollection { get; init; }
        string? Vocabulary { get; init; }
    }
}