using fh_service_directory_api.core.Interfaces.Entities;
using fh_service_directory_api.core.Interfaces.Entities.Aggregates;
using LocalAuthorityInformationServices.SharedKernel;

namespace fh_service_directory_api.core.Concretions.Entities;

public class ServiceTaxonomy : EntityBase<string>, IServiceTaxonomy
{
    private ServiceTaxonomy() { }

    public ServiceTaxonomy
    (
        string id,
        string? linkId,
        ITaxonomy? taxonomy
    )
    {
        Id = id;
        LinkId = linkId;
        Taxonomy = taxonomy;
    }
    public string? LinkId { get; init; }

    public ITaxonomy? Taxonomy { get; init; }
}
