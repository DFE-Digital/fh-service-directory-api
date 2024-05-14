namespace FamilyHubs.ServiceDirectory.Data;
using Enums = FamilyHubs.ServiceDirectory.Shared.Enums;

public class ServiceType
{
    public Enums.ServiceType Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
