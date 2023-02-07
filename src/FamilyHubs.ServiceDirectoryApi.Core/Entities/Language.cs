using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Language : EntityBase<string>, IAggregateRoot
{
    private Language() { }
    public Language(
        string id, 
        string name)
    {
        Id = id;
        Name = name;
    }
    public string Name { get; set; } = default!;
    public string ServiceId { get; set; } = default!;
}
