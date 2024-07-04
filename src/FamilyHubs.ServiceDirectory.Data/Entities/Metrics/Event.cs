using System.Collections.ObjectModel;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data;

public class Event
{
    public ServiceDirectorySearchEventType Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<ServiceSearch> ServiceSearches { get; } = new Collection<ServiceSearch>();
}
