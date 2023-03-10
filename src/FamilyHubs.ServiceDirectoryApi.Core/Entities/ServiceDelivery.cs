using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class ServiceDelivery : EntityBase<long>, IAggregateRoot
{
    public required ServiceDeliveryType Name { get; set; }
    public long ServiceId { get; set; }
}
