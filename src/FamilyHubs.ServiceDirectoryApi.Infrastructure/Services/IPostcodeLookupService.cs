using FamilyHubs.ServiceDirectory.Shared.Models.Api.Postcodes;

namespace fh_service_directory_api.infrastructure.Services
{
    public interface IPostcodeLookupService
    {
        Task<IPostcodeIOResponseDto> GetPostcodeAsync(string postcode);
    }
}
