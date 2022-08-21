using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralServiceDeliveryRecord
{
    private OpenReferralServiceDeliveryRecord() { }
    public OpenReferralServiceDeliveryRecord(string id, ServiceDelivery serviceDelivery)
    {
        Id = id;
        ServiceDelivery = serviceDelivery;
    }

    public string Id { get; init; } = default!;
    public ServiceDelivery ServiceDelivery { get; init; }
}
