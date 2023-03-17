using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Contact : ServiceLocationSharedEntityBase
{
    public string? Title { get; set; }
    public string? Name { get; set; }
    public required string Telephone { get; set; }
    public string? TextPhone { get; set; }
    public string? Url { get; set; }
    public string? Email { get; set; }
}
