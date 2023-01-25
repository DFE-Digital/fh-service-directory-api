using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralLinkTaxonomy : EntityBase<string>, IAggregateRoot
{
    private OpenReferralLinkTaxonomy() { }
    public OpenReferralLinkTaxonomy(string id, string linkId, string linkType, OpenReferralTaxonomy? taxonomy)
    {
        Id = id;
        Taxonomy = taxonomy;
        LinkId = linkId;
        LinkType = linkType;
    }
    public OpenReferralTaxonomy? Taxonomy { get; set; }
    public string LinkId { get; set; } = default!;
    public string LinkType { get; set; } = default!;
}
