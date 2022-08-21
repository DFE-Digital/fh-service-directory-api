using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralServiceDelivery : EntityBase<string>, IOpenReferralServiceDelivery
{
    private OpenReferralServiceDelivery() { }
    public OpenReferralServiceDelivery(string id, ServiceDelivery serviceDelivery)
    {
        Id = id;
        ServiceDelivery = serviceDelivery;
    }

    public ServiceDelivery ServiceDelivery { get; private set; }
}
