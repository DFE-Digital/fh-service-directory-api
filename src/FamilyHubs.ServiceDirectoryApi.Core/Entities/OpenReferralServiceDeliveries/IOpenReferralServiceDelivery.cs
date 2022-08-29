using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceDeliveries;

public interface IOpenReferralServiceDelivery : IEntityBase<string>
{
    ServiceDelivery ServiceDelivery { get; }
}