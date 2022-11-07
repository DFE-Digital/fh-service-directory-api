using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.api.Queries.GetOpenReferralService;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            //TODO: SR - Add code to return Family Hubs

            /*
             SQL Query:

                select 
                    --* 
                    o.Id 'OrgId', ad.Code 'AdminDistrict', s.Name 'Service', l.Id 'LocaId', l.Name 'Location', l.Longitude, l.Latitude, a.Address_1, a.City, a.Postal_code, s.Email
                from 
                    OpenReferralLocations l
                    LEFT OUTER JOIN OpenReferralPhysical_Addresses a ON l.Id = a.OpenReferralLocationId
                    LEFT OUTER JOIN OpenReferralServiceAtLocations sal ON l.Id = sal.LocationId
                    LEFT OUTER JOIN OpenReferralServices s ON sal.OpenReferralServiceId = s.Id
                    LEFT OUTER JOIN OpenReferralOrganisations o ON s.OpenReferralOrganisationId = o.Id
                    LEFT OUTER JOIN OrganisationAdminDistricts ad ON o.Id = ad.OpenReferralOrganisationId
                    LEFT OUTER JOIN ModelLinks ml ON l.Id = ml.ModelOneId
                    LEFT OUTER JOIN OpenReferralTaxonomies t ON ml.ModelTwoId = t.Id
                WHERE
                    --ad.Code = 'E09000030' -- Tower Hamlets
                    t.Name = 'FamilyHub'
             */

            return await Task.FromResult(new List<OpenReferralServiceDto>());

            //var entities = await _context.OpenReferralLocations.Include(l => l.);

            //var result = OpenReferralDtoHelper.GetFamilyHubsDto(entity);
            //return result;
        }
    }
}
