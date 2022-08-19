using fh_service_directory_api.core.Concretions.Entities;
using fh_service_directory_api.core.Interfaces.Entities.Aggregates;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IParent
    {
        ICollection<LinkTaxonomyCollection>? LinkTaxonomyCollection { get; init; }
        string Name { get; init; }
        ICollection<ITaxonomy>? ServiceTaxonomyCollection { get; init; }
        string? Vocabulary { get; init; }
    }
}