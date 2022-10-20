using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OrganisationAdminDistrict : EntityBase<string>, IOrganisationAdminDistrict, IAggregateRoot
{
    private OrganisationAdminDistrict() { }
    public OrganisationAdminDistrict(string id, string code, string openReferralOrganisationId)
    {
        Id = id;
        Code = code;
        OpenReferralOrganisationId = openReferralOrganisationId;
    }

    public string Code { get; set; } = default!;
    public string OpenReferralOrganisationId { get; set; } = default!;
}
