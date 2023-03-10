using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Contact : EntityBase<long>, IAggregateRoot
{
    public string? Title { get; set; }
    public string? Name { get; set; }
    public required string Telephone { get; set; }
    public string? TextPhone { get; set; }
    public string? Url { get; set; }
    public string? Email { get; set; }
}
