namespace FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;

public class ServiceAtLocation
{
    public required long ServiceId { get; set; }
    public required long LocationId { get; set; }
}
