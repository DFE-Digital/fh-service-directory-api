using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities
{
    public interface IOpenReferralOrganisation
    {
        string? Description { get; }
        string? Logo { get; }
        string Name { get; }
        ICollection<IOpenReferralReview>? Reviews { get; set; }
        ICollection<IOpenReferralService>? Services { get; set; }
        string? Uri { get; }
        string? Url { get; }

        void Update(IOpenReferralOrganisation openReferralOpenReferralOrganisation);
    }
}