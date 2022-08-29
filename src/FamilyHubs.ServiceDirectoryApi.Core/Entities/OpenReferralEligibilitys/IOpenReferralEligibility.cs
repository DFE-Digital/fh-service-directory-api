using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralTaxonomies;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralEligibilitys
{
    public interface IOpenReferralEligibility
    {
        string Eligibility { get; init; }
        string? LinkId { get; init; }
        int Maximum_age { get; init; }
        int Minimum_age { get; init; }
        ICollection<OpenReferralTaxonomy>? Taxonomys { get; set; }
    }
}