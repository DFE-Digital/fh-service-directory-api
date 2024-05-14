using System.Collections.ObjectModel;
using Enums = FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data;

public class ServiceType
{
    public Enums.ServiceType Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<ServiceSearch> ServiceSearches { get; } = new Collection<ServiceSearch>();
}
