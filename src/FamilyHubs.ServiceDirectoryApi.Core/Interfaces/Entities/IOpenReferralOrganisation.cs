namespace fh_service_directory_api.core.Interfaces.Entities;

public interface IOpenReferralOrganisation : IEntityBase<string>
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