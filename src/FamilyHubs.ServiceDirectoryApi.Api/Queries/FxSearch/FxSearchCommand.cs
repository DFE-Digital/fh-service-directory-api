using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;

namespace fh_service_directory_api.api.Queries.FxSearch
{
    public class FxSearchCommand : IRequest<List<Either<OpenReferralServiceDto, OpenReferralLocationDto, double>>>
    {
        public FxSearchCommand(string? districtCode, double? longtitude, double? latitude)
        {
            DistrictCode = districtCode;
            Longtitude = longtitude;
            Latitude = latitude;
        }

        public string? DistrictCode { get; }
        public double? Longtitude { get; }
        public double? Latitude { get; }
    }

    public class FxSearchCommandHandler : IRequestHandler<FxSearchCommand, List<Either<OpenReferralServiceDto, OpenReferralLocationDto, double>>>
    {
        private readonly ApplicationDbContext _context;

        public FxSearchCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Either<OpenReferralServiceDto, OpenReferralLocationDto, double>>> Handle(FxSearchCommand request, CancellationToken cancellationToken)
        {
            List<Either<OpenReferralServiceDto, OpenReferralLocationDto, double>> resultList = new();

            List<OpenReferralLocation> familyHubs = GetFamilyHubLocations(request.DistrictCode ?? string.Empty);


            foreach (var familyHub in familyHubs)
            {
                double distance = 0.0D;
                if (request?.Latitude != null && request?.Longtitude != null)
                {
                    distance = core.Helper.GetDistance(request.Latitude, request.Longtitude, familyHub.Latitude, familyHub.Longitude);
                }

                List<OpenReferralPhysicalAddressDto> physicalAddresses = new();
                if (familyHub.Physical_addresses != null)
                {
                    foreach (var address in familyHub.Physical_addresses)
                    {
                        physicalAddresses.Add(new OpenReferralPhysicalAddressDto(address.Id, address.Address_1, address.City, address.Postal_code, address.Country, address.State_province));
                    }
                }

                resultList.Add(new Either<OpenReferralServiceDto, OpenReferralLocationDto, double>
                {
                    Second = new OpenReferralLocationDto(
                    familyHub.Id, familyHub.Name, familyHub.Description, familyHub.Latitude, familyHub.Longitude, physicalAddresses),
                    Third = distance
                });
            }


            List<string> organisationIds = await _context.OrganisationAdminDistricts.Where(x => x.Code == request.DistrictCode).Select(x => x.OpenReferralOrganisationId).ToListAsync();

            List<OpenReferralService> entities = await _context.OpenReferralServices
                  .Include(x => x.ServiceType)
                  .Include(x => x.ServiceDelivery)
                  .Include(x => x.Eligibilities)
                  .Include(x => x.Contacts)
                  .ThenInclude(x => x.Phones)
                  .Include(x => x.Languages)
                  .Include(x => x.Service_areas)
                  .Include(x => x.Service_taxonomys)
                  .ThenInclude(x => x.Taxonomy)
                  .Include(x => x.Service_at_locations)
                  .ThenInclude(x => x.Location)
                  .ThenInclude(x => x.Physical_addresses)
                  .Include(x => x.Cost_options)
                  .Where(x => x.Status != "Deleted" && organisationIds.Contains(x.OpenReferralOrganisationId)).ToListAsync();

            var filteredServices = OpenReferralDtoHelper.GetOpenReferralServicesDto(entities);
            foreach (var service in filteredServices)
            {
                double distance = 0.0D;
                if (request?.Latitude != null && request?.Longtitude != null && service.Service_at_locations != null)
                {
                    foreach (var serviceAtLocation in service.Service_at_locations)
                    {
                        double currentdistance = core.Helper.GetDistance(request.Latitude, request.Longtitude, serviceAtLocation.Location.Latitude, serviceAtLocation.Location.Longitude);
                        if (distance < currentdistance)
                            distance = currentdistance;
                    }
                }

                resultList.Add(new Either<OpenReferralServiceDto, OpenReferralLocationDto, double> { First = service, Third = distance });
            }

            return resultList;
        }

        private List<OpenReferralLocation> GetFamilyHubLocations(string districtCode)
        {
            var results = from loc in _context.OpenReferralLocations.Include(x => x.Physical_addresses)
                          join model in _context.ModelLinks on loc.Id equals model.ModelOneId
                          join orgdist in _context.OrganisationAdminDistricts on model.ModelTwoId equals orgdist.OpenReferralOrganisationId
                          where orgdist.Code == districtCode
                          select loc;

            return results.ToList();

        }
    }
}
