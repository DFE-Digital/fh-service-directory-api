using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Taxonomy : EntityBase<long>, IAggregateRoot
{
    public required string Name { get; set; }
    public TaxonomyType TaxonomyType { get; set; }
    public long? ParentId { get; set; }
}
