using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAreas;

public class OpenReferralServiceArea : EntityBase<string>, IOpenReferralServiceArea, IAggregateRoot
{
    private OpenReferralServiceArea() { }
    public OpenReferralServiceArea(string id, string service_area, string? linkId, string? extent, string? uri)
    {
        Id = id;
        Service_area = service_area;
        LinkId = linkId;
        Extent = extent;
        Uri = uri;
    }
    public string Service_area { get; init; } = default!;
    public string? LinkId { get; init; } = default!;
    public string? Extent { get; init; }
    public string? Uri { get; init; }
}
