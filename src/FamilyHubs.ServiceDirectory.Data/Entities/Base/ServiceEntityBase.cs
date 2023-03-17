namespace FamilyHubs.ServiceDirectory.Data.Entities.Base;

public class ServiceEntityBase<TId> : EntityBase<TId>
{
    public TId ServiceId { get; set; } = default!;
}