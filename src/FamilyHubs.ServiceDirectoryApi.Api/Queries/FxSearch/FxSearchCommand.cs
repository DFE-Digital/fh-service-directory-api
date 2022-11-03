using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.api.Queries.GetOpenReferralService;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Queries.FxSearch
{
    public class FxSearchCommand : IRequest<List<OpenReferralServiceDto>>
    {
        public FxSearchCommand(string? postcode, string? districtCode, double? longtitude, double? latitude)
        {
            Postcode = postcode;
            DistrictCode = districtCode;
            Longtitude = longtitude;
            Latitude = latitude;
        }

        public string? Postcode { get; }
        public string? DistrictCode { get; }
        public double? Longtitude { get; }
        public double? Latitude { get; }
    }

    public class FxSearchCommandHandler : IRequestHandler<FxSearchCommand, List<OpenReferralServiceDto>>
    {
        private readonly ApplicationDbContext _context;

        public FxSearchCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OpenReferralServiceDto>> Handle(FxSearchCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<OpenReferralServiceDto>());

            //var entities = await _context.;

            //var result = OpenReferralDtoHelper.GetOpenReferralServiceDto(entity);
            //return result;
        }
    }
}
