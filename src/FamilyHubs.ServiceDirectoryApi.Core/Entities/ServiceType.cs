using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class ServiceType : EntityBase<string>, IServiceType, IAggregateRoot
{
    private ServiceType() { }
    public ServiceType(string id, string name, string? description)
    {
        Id = id; //use number 1 -> 99 in a string
        Name = name;
        Description = description;
    }
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
}
