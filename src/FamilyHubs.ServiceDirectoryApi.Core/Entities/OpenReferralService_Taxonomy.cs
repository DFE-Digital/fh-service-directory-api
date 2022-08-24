using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralService_Taxonomy : EntityBase<string>, IOpenReferralService_Taxonomy, IAggregateRoot
{
    private OpenReferralService_Taxonomy() { }
    public OpenReferralService_Taxonomy(string id, string? linkId, OpenReferralTaxonomy? taxonomy)
    {
        Id = id;
        LinkId = linkId;
        Taxonomy = taxonomy;
    }
    public string? LinkId { get; init; }
    public OpenReferralTaxonomy? Taxonomy { get; set; }
}
