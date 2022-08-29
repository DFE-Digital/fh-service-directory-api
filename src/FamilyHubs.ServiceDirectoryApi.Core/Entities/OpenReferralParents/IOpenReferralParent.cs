using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLinkTaxonomyCollections;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralParents;

public interface IOpenReferralParent : IEntityBase<string>
{
    ICollection<OpenReferralLinktaxonomycollection>? LinkTaxonomyCollection { get; init; }
    string Name { get; init; }
    ICollection<OpenReferralServiceTaxonomies.OpenReferralServiceTaxonomy>? ServiceTaxonomyCollection { get; init; }
    string? Vocabulary { get; init; }
}