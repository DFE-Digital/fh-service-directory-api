using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;

public class ServiceAtLocation : EntityBase<long>
{
    public long ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    public long LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public string? Description { get; set; }

    public IList<Schedule> Schedules { get; set; } = new List<Schedule>();
}