using FamilyHubs.ServiceDirectory.Shared.Models.Api.Postcodes;

namespace fh_service_directory_api.infrastructure.Services
{
    public interface IPostcodeLookupService
    {
        Task<PostcodeIOResponseDto> GetPostcodeAsync(string postcode);
    }
}
