using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class AdminArea : EntityBase<string>, IAggregateRoot
{
    private AdminArea() { }
    public AdminArea(string id, string code, string openReferralOrganisationId)
    {
        Id = id;
        Code = code;
        OpenReferralOrganisationId = openReferralOrganisationId;
    }

    public string Code { get; set; } = default!;
    public string OpenReferralOrganisationId { get; set; } = default!;
}
