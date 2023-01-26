using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class OrganisationType : EntityBase<string>, IAggregateRoot
{
    private OrganisationType() { }
    public OrganisationType(string id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}