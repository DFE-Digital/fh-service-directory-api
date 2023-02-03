using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Funding : EntityBase<string>, IAggregateRoot
{
    private Funding() { }
    public Funding(
        string id, 
        string source)
    {
        Id = id;
        Source = source;
    }
    public string Source { get; set; } = default!;
    public string ServiceId { get; set; } = default!;
}
