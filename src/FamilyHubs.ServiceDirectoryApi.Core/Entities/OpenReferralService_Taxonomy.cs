using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralService_Taxonomy : EntityBase<string>, IOpenReferralService_Taxonomy, IAggregateRoot
{
    private OpenReferralService_Taxonomy() { }
    public OpenReferralService_Taxonomy(string id, string? linkId, IOpenReferralTaxonomy? taxonomy)
    {
        Id = id;
        LinkId = linkId;
        Taxonomy = taxonomy;
    }
    public string? LinkId { get; init; }
    public IOpenReferralTaxonomy? Taxonomy { get; set; }
}
