using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralParent : EntityBase<string>, IOpenReferralParent, IAggregateRoot
{
    private OpenReferralParent() { }
    public OpenReferralParent(string id, string name, string? vocabulary, ICollection<IOpenReferralService_Taxonomy>? serviceTaxonomyCollection, ICollection<IOpenReferralLinktaxonomycollection>? linkTaxonomyCollection)
    {
        Id = id;
        Name = name;
        Vocabulary = vocabulary;
        ServiceTaxonomyCollection = serviceTaxonomyCollection;
        LinkTaxonomyCollection = linkTaxonomyCollection;
    }
    public string Name { get; init; } = default!;
    public string? Vocabulary { get; init; }
    public virtual ICollection<IOpenReferralService_Taxonomy>? ServiceTaxonomyCollection { get; init; }
    public virtual ICollection<IOpenReferralLinktaxonomycollection>? LinkTaxonomyCollection { get; init; }
}
