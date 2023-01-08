using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralLinkTaxonomy : EntityBase<string>, IOpenReferralLinkTaxonomy, IAggregateRoot
{
    private OpenReferralLinkTaxonomy() { }
    public OpenReferralLinkTaxonomy(string id, string linkId, string linkType, OpenReferralTaxonomy? taxonomy)
    {
        Id = id;
        LinkId = linkId;
        LinkType = linkType;
        Taxonomy = taxonomy;
    }
    public string LinkId { get; set; } = default!;
    public string LinkType { get; set; } = default!;
    public OpenReferralTaxonomy? Taxonomy { get; set; }
}
