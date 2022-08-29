namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralReviews;

public interface IOpenReferralReview : IEntityBase<string>
{
    DateTime Date { get; }
    string? Description { get; }
    string? Score { get; }
    string Title { get; }
    string? Url { get; }
    string? Widget { get; }
    string OpenReferralServiceId { get; }
    void Update(IOpenReferralReview openReferralReview);
}