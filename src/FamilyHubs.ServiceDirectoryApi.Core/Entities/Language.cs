using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Language : EntityBase<long>, IAggregateRoot
{
    public required string Name { get; set; }
    public long ServiceId { get; set; }
}
