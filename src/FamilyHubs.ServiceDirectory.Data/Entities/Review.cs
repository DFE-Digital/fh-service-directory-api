using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Review : OrganisationServiceSharedEntityBase
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required DateTime Date { get; set; }
    public string? Score { get; set; }
    public string? Url { get; set; }
    public string? Widget { get; set; }
}
