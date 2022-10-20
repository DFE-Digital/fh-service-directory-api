namespace fh_service_directory_api.core.Interfaces.Entities;

public interface IOrganisationAdminDistrict : IEntityBase<string>
{
    string Code { get; set; }
    string OpenReferralOrganisationId { get; set; }
}
