using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Taxonomy : EntityBase<long>
{
    public required string Name { get; set; }
    public TaxonomyType TaxonomyType { get; set; }
    public long? ParentId { get; set; }
}
