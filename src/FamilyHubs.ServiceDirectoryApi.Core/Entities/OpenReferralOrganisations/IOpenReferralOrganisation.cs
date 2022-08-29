using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralReviews;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServices;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;

public interface IOpenReferralOrganisation
{
    string? Description { get; }
    string? Logo { get; }
    string Name { get; }
    ICollection<OpenReferralReview>? Reviews { get; set; }
    ICollection<OpenReferralService>? Services { get; set; }
    string? Uri { get; }
    string? Url { get; }

    void Update(IOpenReferralOrganisation openReferralOpenReferralOrganisation);
}