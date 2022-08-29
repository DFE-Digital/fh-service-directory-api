using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralTaxonomies;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceTaxonomies;

public class OpenReferralServiceTaxonomy : EntityBase<string>, IOpenReferralService_Taxonomy, IAggregateRoot
{
    private OpenReferralServiceTaxonomy() { }
    public OpenReferralServiceTaxonomy(string id, string? linkId, OpenReferralTaxonomy? taxonomy)
    {
        Id = id;
        LinkId = linkId;
        Taxonomy = taxonomy;
    }
    public string? LinkId { get; init; }
    public OpenReferralTaxonomy? Taxonomy { get; set; }
}
