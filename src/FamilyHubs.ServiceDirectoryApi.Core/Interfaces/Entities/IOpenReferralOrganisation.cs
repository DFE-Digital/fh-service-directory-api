using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
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
}