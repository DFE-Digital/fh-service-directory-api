using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralParent : EntityBase<string>, IAggregateRoot
{
    private OpenReferralParent() { }
    public OpenReferralParent(string id, string name, string? vocabulary, ICollection<OpenReferralService_Taxonomy>? serviceTaxonomyCollection, ICollection<OpenReferralLinkTaxonomy>? linkTaxonomyCollection)
    {
        Id = id;
        Name = name;
        Vocabulary = vocabulary;
        ServiceTaxonomyCollection = serviceTaxonomyCollection;
        LinkTaxonomyCollection = linkTaxonomyCollection;
    }
    public string Name { get; init; } = default!;
    public string? Vocabulary { get; init; }
    public virtual ICollection<OpenReferralService_Taxonomy>? ServiceTaxonomyCollection { get; init; }
    public virtual ICollection<OpenReferralLinkTaxonomy>? LinkTaxonomyCollection { get; init; }
}
