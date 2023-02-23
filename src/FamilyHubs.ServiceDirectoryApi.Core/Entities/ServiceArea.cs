using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class ServiceArea : EntityBase<string>, IAggregateRoot
{
    private ServiceArea() { }
    public ServiceArea(
        string id, 
        string serviceAreaDescription, 
        string? linkId, 
        string? extent, 
        string? uri)
    {
        Id = id;
        ServiceAreaDescription = serviceAreaDescription;
        LinkId = linkId;
        Extent = extent;
        Uri = uri;
    }
    public string ServiceAreaDescription { get; set; } = default!;
    public string? LinkId { get; set; }
    public string? Extent { get; set; }
    public string? Uri { get; set; }
    public string ServiceId { get; set; } = default!;
}
