namespace FamilyHubs.ServiceDirectory.Data.Entities.Base;

public class OrganisationServiceSharedEntityBase : EntityBase<long>
{
    public long? OrganisationId { get; set; } = default!;
    public long? ServiceId { get; set; } = default!;
}