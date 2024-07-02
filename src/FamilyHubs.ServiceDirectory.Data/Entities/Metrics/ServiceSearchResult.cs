using FamilyHubs.ServiceDirectory.Data.Entities;

namespace FamilyHubs.ServiceDirectory.Data;

public class ServiceSearchResult
{
    public long Id { get; set; }
    public long ServiceId { get; set; }
    public required Service Service { get; set; } 
    public long ServiceSearchId { get; set; }
}
