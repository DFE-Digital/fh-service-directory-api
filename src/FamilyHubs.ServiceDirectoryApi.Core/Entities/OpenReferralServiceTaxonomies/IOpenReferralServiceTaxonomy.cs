using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralTaxonomies;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceTaxonomies;

public interface IOpenReferralService_Taxonomy : IEntityBase<string>
{
    string? LinkId { get; init; }
    OpenReferralTaxonomy? Taxonomy { get; set; }
}