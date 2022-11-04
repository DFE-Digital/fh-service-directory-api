using FamilyHubs.ServiceDirectory.Shared.Models.Api.Postcodes;
using fh_service_directory_api.infrastructure.Services;
using MediatR;
using System.Net;

namespace fh_service_directory_api.api.Queries.GetPostcode
{
    public class GetPostcodeCommand : IRequest<PostcodeIOResponseDto>
    {
        public GetPostcodeCommand(string? postcodeToFind)
        {
            PostcodeToFind = postcodeToFind;
        }

        public string? PostcodeToFind { get; }
    }

    public class GetPostcodeCommandHandler : IRequestHandler<GetPostcodeCommand, PostcodeIOResponseDto>
    {
        private readonly IPostcodeLookupService _postcodeLookupService;

        public GetPostcodeCommandHandler(IPostcodeLookupService postcodeLookupService)
        {
            _postcodeLookupService = postcodeLookupService;
        }

        public async Task<PostcodeIOResponseDto> Handle(GetPostcodeCommand request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.PostcodeToFind))
            {
                return new PostcodeIOResponseDto { Status = HttpStatusCode.NotFound.ToString() };
            }

            var result = await _postcodeLookupService.GetPostcodeAsync(request.PostcodeToFind);

            return result;
        }
    }
}
