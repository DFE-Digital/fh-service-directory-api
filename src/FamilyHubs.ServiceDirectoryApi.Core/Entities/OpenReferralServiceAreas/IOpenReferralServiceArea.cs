namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAreas;

public interface IOpenReferralServiceArea : IEntityBase<string>
{
    string? Extent { get; init; }
    string? LinkId { get; init; }
    string Service_area { get; init; }
    string? Uri { get; init; }
}