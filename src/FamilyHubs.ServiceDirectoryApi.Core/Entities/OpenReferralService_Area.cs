﻿using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralService_Area : EntityBase<string>, IAggregateRoot
{
    private OpenReferralService_Area() { }
    public OpenReferralService_Area(string id, string service_area, string? linkId, string? extent, string? uri)
    {
        Id = id;
        Service_area = service_area;
        LinkId = linkId;
        Extent = extent;
        Uri = uri;
    }
    public string Service_area { get; set; } = default!;
    public string? LinkId { get; set; }
    public string? Extent { get; set; }
    public string? Uri { get; set; }
    public string OpenReferralServiceId { get; set; } = default!;
}
