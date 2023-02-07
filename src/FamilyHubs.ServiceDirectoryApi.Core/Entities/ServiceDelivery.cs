using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class ServiceDelivery : EntityBase<string>, IAggregateRoot
{
    private ServiceDelivery() { }
    public ServiceDelivery(
        string id, 
        ServiceDeliveryType name)
    {
        Id = id;
        Name = name;
    }

    public ServiceDeliveryType Name { get; set; }
    public string ServiceId { get; set; } = default!;
}
