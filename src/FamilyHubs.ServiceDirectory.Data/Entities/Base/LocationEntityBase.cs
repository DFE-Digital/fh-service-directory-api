namespace FamilyHubs.ServiceDirectory.Data.Entities.Base;

public class LocationEntityBase<TId> : EntityBase<TId>
{
    public TId LocationId { get; set; } = default!;
}