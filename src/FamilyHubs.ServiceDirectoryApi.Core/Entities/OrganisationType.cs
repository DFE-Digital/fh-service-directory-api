using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OrganisationType : EntityBase<string>, IOrganisationType, IAggregateRoot
{
    private OrganisationType() { }
    public OrganisationType(string id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
}