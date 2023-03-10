using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Review : EntityBase<long>, IAggregateRoot
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required DateTime Date { get; set; }
    public string? Score { get; set; }
    public string? Url { get; set; }
    public string? Widget { get; set; }
    public long? ServiceId { get; set; }
    public long? OrganisationId { get; set; }
}
