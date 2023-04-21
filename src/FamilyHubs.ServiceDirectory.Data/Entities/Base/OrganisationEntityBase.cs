namespace FamilyHubs.ServiceDirectory.Data.Entities.Base;

public class OrganisationEntityBase<TId> : EntityBase<TId>
{
    public required TId OrganisationId { get; set; }
}