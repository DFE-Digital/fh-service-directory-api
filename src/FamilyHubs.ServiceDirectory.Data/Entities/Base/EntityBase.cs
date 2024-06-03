namespace FamilyHubs.ServiceDirectory.Data.Entities.Base;


/// <summary>
/// Base types for all Entities which track state using a given Id.
/// </summary>
public abstract class EntityBase<TId>
{
    public TId Id { get; set; } = default!;

    public DateTime? Created { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public long? LastModifiedBy { get; set; }
}
