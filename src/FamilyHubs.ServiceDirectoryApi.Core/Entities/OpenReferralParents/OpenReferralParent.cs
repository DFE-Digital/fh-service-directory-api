using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLinkTaxonomyCollections;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceTaxonomies;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralParents;

public class OpenReferralParent : EntityBase<string>, IOpenReferralParent, IAggregateRoot
{
    private OpenReferralParent() { }
    public OpenReferralParent(string id, string name, string? vocabulary, ICollection<IOpenReferralService_Taxonomy>? serviceTaxonomyCollection, ICollection<IOpenReferralLinktaxonomycollection>? linkTaxonomyCollection)
    {
        Id = id;
        Name = name;
        Vocabulary = vocabulary;
        ServiceTaxonomyCollection = serviceTaxonomyCollection as ICollection<OpenReferralServiceTaxonomy>;
        LinkTaxonomyCollection = linkTaxonomyCollection as ICollection<OpenReferralLinktaxonomycollection>;
    }
    public string Name { get; init; } = default!;
    public string? Vocabulary { get; init; }
    public virtual ICollection<OpenReferralServiceTaxonomy>? ServiceTaxonomyCollection { get; init; }
    public virtual ICollection<OpenReferralLinktaxonomycollection>? LinkTaxonomyCollection { get; init; }
}
