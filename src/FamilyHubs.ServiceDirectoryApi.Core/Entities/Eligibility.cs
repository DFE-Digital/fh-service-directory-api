using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Eligibility : EntityBase<long>, IAggregateRoot
{ 
    public EligibilityType EligibilityType { get; set; }
    public required int MaximumAge { get; set; }
    public required int MinimumAge { get; set; }
    public long ServiceId { get; set; }
    public ICollection<Taxonomy> Taxonomies { get; set; } = new List<Taxonomy>();
}
