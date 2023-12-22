using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Language : ServiceEntityBase<long>
{
    // the standard says Name and Code aren't required, but we're going to require them
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string? Note { get; set; }
}
