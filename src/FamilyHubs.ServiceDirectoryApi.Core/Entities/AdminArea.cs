using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class AdminArea : EntityBase<string>, IAggregateRoot
{
    private AdminArea() { }
    public AdminArea(
        string id, 
        string code, 
        string organisationId)
    {
        Id = id;
        Code = code;
        OrganisationId = organisationId;
    }

    public string Code { get; set; } = default!;
    public string OrganisationId { get; set; } = default!;
}
