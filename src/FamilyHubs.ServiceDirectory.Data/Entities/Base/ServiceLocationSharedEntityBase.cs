namespace FamilyHubs.ServiceDirectory.Data.Entities.Base;

public class ServiceLocationSharedEntityBase : EntityBase<long>
{
    public long? ServiceId { get; set; }
    public long? LocationId { get; set; }
}