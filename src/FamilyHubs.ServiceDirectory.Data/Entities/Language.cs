using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Language : ServiceEntityBase<long>
{
    public required string Name { get; set; }
}
