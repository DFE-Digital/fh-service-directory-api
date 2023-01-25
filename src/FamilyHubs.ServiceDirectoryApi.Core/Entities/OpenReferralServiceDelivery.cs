using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralServiceDelivery : EntityBase<string>, IAggregateRoot
{
    private OpenReferralServiceDelivery() { }
    public OpenReferralServiceDelivery(string id, ServiceDelivery serviceDelivery)
    {
        Id = id;
        ServiceDelivery = serviceDelivery;
    }

    public ServiceDelivery ServiceDelivery { get; set; }
    public string OpenReferralServiceId { get; set; } = default!;
}
