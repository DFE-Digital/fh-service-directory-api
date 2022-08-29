namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLinkTaxonomyCollections;

public interface IOpenReferralLinktaxonomycollection : IEntityBase<string>
{
    string Link_id { get; init; }
    string Link_type { get; init; }
}