using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Funding : ServiceEntityBase<long>
{
    public string? Source { get; set; }
}
