using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLinkTaxonomyCollections;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralTaxonomies;

public interface IOpenReferralTaxonomy : IEntityBase<string>
{
    ICollection<OpenReferralLinktaxonomycollection>? LinkTaxonomyCollection { get; init; }
    string Name { get; init; }
    string? Parent { get; init; }
    string? Vocabulary { get; init; }
}