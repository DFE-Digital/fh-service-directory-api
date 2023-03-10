using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Funding : EntityBase<long>, IAggregateRoot
{
    public string? Source { get; set; }
    public long ServiceId { get; set; }
}
