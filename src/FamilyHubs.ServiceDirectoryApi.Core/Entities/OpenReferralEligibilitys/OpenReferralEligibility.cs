using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralTaxonomies;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralEligibilitys;

public class OpenReferralEligibility : EntityBase<string>, IAggregateRoot, IOpenReferralEligibility
{
    private OpenReferralEligibility() { }
    public OpenReferralEligibility(string id, string eligibility, string? linkId, int maximum_age, int minimum_age, ICollection<OpenReferralTaxonomy>? taxonomys)
    {
        Id = id;
        Eligibility = eligibility;
        LinkId = linkId;
        Maximum_age = maximum_age;
        Minimum_age = minimum_age;
        Taxonomys = taxonomys;
    }
    public string Eligibility { get; init; } = default!;
    public string? LinkId { get; init; }
    public int Maximum_age { get; init; }
    public int Minimum_age { get; init; }
    public ICollection<OpenReferralTaxonomy>? Taxonomys { get; set; }
}
