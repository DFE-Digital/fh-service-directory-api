using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class ServiceArea : EntityBase<long>, IAggregateRoot
{ 
    public string? ServiceAreaName { get; set; }
    public string? Extent { get; set; }
    public string? Uri { get; set; }
    public long ServiceId { get; set; }
    public ICollection<Taxonomy> Taxonomies { get; set; } = new List<Taxonomy>();
}
