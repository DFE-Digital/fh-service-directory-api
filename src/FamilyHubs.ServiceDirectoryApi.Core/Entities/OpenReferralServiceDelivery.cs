using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralServiceDelivery : EntityBase<string>, IOpenReferralServiceDelivery, IAggregateRoot
{
    private OpenReferralServiceDelivery() { }
    public OpenReferralServiceDelivery(string id, ServiceDelivery serviceDelivery)
    {
        Id = id;
        ServiceDelivery = serviceDelivery;
    }

    public ServiceDelivery ServiceDelivery { get; private set; }
}
