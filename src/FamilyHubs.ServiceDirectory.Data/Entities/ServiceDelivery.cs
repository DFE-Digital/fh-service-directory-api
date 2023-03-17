using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class ServiceDelivery : ServiceEntityBase<long>
{
    public required ServiceDeliveryType Name { get; set; }
}
