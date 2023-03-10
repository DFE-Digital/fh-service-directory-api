using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class CostOption : EntityBase<long>, IAggregateRoot
{
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string? Option { get; set; }
    public decimal? Amount { get; set; }
    public string? AmountDescription { get; set; }
    public long ServiceId { get; set; }
    public ICollection<Taxonomy> Taxonomies { get; set; } = new List<Taxonomy>();
}
