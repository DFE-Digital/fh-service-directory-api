using fh_service_directory_api.core.RecordEntities;

namespace fh_service_directory_api.core.Interfaces.Commands
{
    public interface ICreateOpenReferralOrganisationCommand
    {
        OpenReferralOrganisationWithServicesRecord OpenReferralOrganisation { get; init; }
    }
}