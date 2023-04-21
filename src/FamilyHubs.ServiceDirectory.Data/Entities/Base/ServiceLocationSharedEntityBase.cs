namespace FamilyHubs.ServiceDirectory.Data.Entities.Base;

public class ServiceLocationSharedEntityBase : EntityBase<long>
{
    public long? ServiceId { get; set; } = default!;
    public long? LocationId { get; set; } = default!;
}