using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Constants;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetServices;

public class GetServicesCommand : IRequest<PaginatedList<ServiceDto>>
{
    public GetServicesCommand(string? serviceType, string? status, string? districtCode, int? minimum_age, int? maximum_age, int? given_age, double? latitude, double? longtitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonmyIds, string? languages, bool? canFamilyChooseLocation, bool? isFamilyHub, int? maxFamilyHubs)
    {
        ServiceType = serviceType;
        Status = status;
        DistrictCode = districtCode;
        MaximumAge = maximum_age;
        MinimumAge = minimum_age;
        GivenAge = given_age;
        Latitude = latitude;
        Longtitude = longtitude;
        Meters = proximity;
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 10;
        Text = text;
        ServiceDeliveries = serviceDeliveries;
        IsPaidFor = isPaidFor;
        TaxonmyIds = taxonmyIds;
        Languages = languages;
        CanFamilyChooseLocation = canFamilyChooseLocation;
        IsFamilyHub = isFamilyHub;
        MaxFamilyHubs = maxFamilyHubs;
    }

    public string? ServiceType { get; set; }
    public string? Status { get; set; }
    public string? DistrictCode { get; set; }
    public int? MaximumAge { get; set; }
    public int? MinimumAge { get; set; }
    public int? GivenAge { get; set; }
    public double? Latitude { get; set; }
    public double? Longtitude { get; set; }
    public double? Meters { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Text { get; set; }
    public string? ServiceDeliveries { get; set; }
    public bool? IsPaidFor { get; set; }
    public string? TaxonmyIds { get; set; }
    public string? Languages { get; set; }
    public bool? CanFamilyChooseLocation { get; set; }
    public bool? IsFamilyHub { get; set; }
    public int? MaxFamilyHubs { get; set; }
}

public class GetServicesCommandHandler : IRequestHandler<GetServicesCommand, PaginatedList<ServiceDto>>
{
    private readonly ApplicationDbContext _context;

    public GetServicesCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ServiceDto>> Handle(GetServicesCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Status))
            request.Status = "active";

        IQueryable<Service> entities = await GetServices(request);

        if (request.MaximumAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(x => x.MaximumAge <= request.MaximumAge.Value));

        if (request.MinimumAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(x => x.MinimumAge >= request.MinimumAge.Value));

        if (request.GivenAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(x => x.MinimumAge <= request.GivenAge.Value && x.MaximumAge >= request.GivenAge.Value));

        if (!string.IsNullOrEmpty(request.Text))
            entities = entities.Where(x => x.Name.Contains(request.Text) || x.Description != null && x.Description.Contains(request.Text));

        IEnumerable<Service> dbservices = await entities.ToListAsync();
        if (request.Latitude != null && request.Longtitude != null && request.Meters != null)
            dbservices = dbservices.Where(x => Core.Helper.GetDistance(request.Latitude, request.Longtitude, x?.ServiceAtLocations?.FirstOrDefault()?.Location.Latitude, x?.ServiceAtLocations?.FirstOrDefault()?.Location.Longitude, x?.Name) < request.Meters);

        //ServiceDeliveries
        if (!string.IsNullOrEmpty(request.ServiceDeliveries))
        {
            List<Service> servicesFilteredByDelMethod = new List<Service>();
            string[] parts = request.ServiceDeliveries.Split(',');
            
            foreach (var part in parts)
                if (Enum.TryParse(part, true, out ServiceDeliveryType serviceDelivery))
                    servicesFilteredByDelMethod.AddRange(dbservices.Where(x => x.ServiceDeliveries.Any(x => x.Name == serviceDelivery)).ToList());
            
            dbservices = servicesFilteredByDelMethod;
        }

        //Languages
        if (!string.IsNullOrEmpty(request.Languages))
        {
            List<Service> servicesFilteredByLanguages = new List<Service>();
            string[] parts = request.Languages.Split(',');
            
            foreach (var part in parts)
                servicesFilteredByLanguages.AddRange(dbservices.Where(x => x.Languages.Any(x => x.Name == part)).ToList());
            
            dbservices = servicesFilteredByLanguages;
        }

        if (request.IsPaidFor != null)
        {
            dbservices = dbservices.Where(x => IsPaidFor(x) == request.IsPaidFor);
        }

        //Can families choose location
        if (request.CanFamilyChooseLocation is true)
        {
            dbservices = dbservices.Where(x => x.CanFamilyChooseDeliveryLocation);
        }

        if (!string.IsNullOrEmpty(request.TaxonmyIds))
        {
            string[] parts = request.TaxonmyIds.Split(',');
            dbservices = dbservices.Where(x => x.ServiceTaxonomies.Any(x => parts.Contains(x.Taxonomy?.Id) ));
        }

        // filter before we calculate distance and map, for efficiency
        if (request.IsFamilyHub != null)
        {
            dbservices = dbservices.Where(s =>
                s.ServiceAtLocations.FirstOrDefault()?.Location.LinkTaxonomies
                    ?.Any(lt => string.Equals(lt.Taxonomy?.Id, TaxonomyDtoIds.FamilyHub, StringComparison.OrdinalIgnoreCase)) == request.IsFamilyHub);
        }

        //todo: better to filter first, so we don't unnecessarily map?

        //if (request?.Latitude != null && request?.Longtitude != null)
        //    dbservices = dbservices.OrderBy(x => core.Helper.GetDistance(
        //        request.Latitude,
        //        request.Longtitude,
        //        x?.Service_at_locations?.FirstOrDefault()?.Location.Latitude,
        //        x?.Service_at_locations?.FirstOrDefault()?.Location.Longitude,
        //        x?.Name));

        var filteredServices = DtoHelper.GetServicesDto(dbservices);
        if (request.Latitude != null && request.Longtitude != null)
        {
            foreach (var service in filteredServices)
            {
                service.Distance = Core.Helper.GetDistance(
                    request.Latitude,
                    request.Longtitude,
                    service.ServiceAtLocations?.FirstOrDefault()?.Location.Latitude,
                    service.ServiceAtLocations?.FirstOrDefault()?.Location.Longitude);
            }

            filteredServices = filteredServices.OrderBy(x => x.Distance).ToList();
        }

        if (request.IsFamilyHub is null && request.MaxFamilyHubs != null)
        {
            // MaxFamilyHubs is really a flag to only include the nearest max family hubs at the start of the results set (when not filtering by IsFamilyHub)
            filteredServices = (filteredServices.Where(IsFamilyHub).Take(request.MaxFamilyHubs.Value)
                .Concat(filteredServices.Where(s => !IsFamilyHub(s)))).ToList();
        }

        var pagelist = filteredServices.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
        var result = new PaginatedList<ServiceDto>(pagelist, filteredServices.Count, request.PageNumber, request.PageSize);
        return result;
    }

    private bool IsPaidFor(Service service)
    {
        if (!service.CostOptions.Any())
            return false;

        return !(service.CostOptions.Any(co => co.Amount == decimal.Zero && string.Equals(co.Option, "Free", StringComparison.OrdinalIgnoreCase)));
    }

    private bool IsFamilyHub(ServiceDto service)
    {
        return service.ServiceAtLocations?.FirstOrDefault()?.Location.LinkTaxonomies
            ?.Any(lt => string.Equals(lt.Taxonomy?.Id, TaxonomyDtoIds.FamilyHub, StringComparison.OrdinalIgnoreCase)) == true;
    }

    private async Task<IQueryable<Service>> GetServices(GetServicesCommand request)
    {
        IQueryable<Service> services;

        if (request.DistrictCode != null)
        {
            List<string> organisationIds = await _context.AdminAreas.Where(x => x.Code == request.DistrictCode).Select(x => x.OrganisationId).ToListAsync();

           services = _context.Services
          .Include(x => x.ServiceType)
          .Include(x => x.ServiceDeliveries)
          .Include(x => x.Eligibilities)
          .Include(x => x.LinkContacts)
          .Include(x => x.Languages)
          .Include(x => x.ServiceAreas)
          .Include(x => x.ServiceTaxonomies)
          .ThenInclude(x => x.Taxonomy)
          
          .Include(x => x.ServiceAtLocations)
          .ThenInclude(x => x.Location)
          .ThenInclude(x => x.PhysicalAddresses)

          .Include(x => x.ServiceAtLocations)
          .ThenInclude(x => x.Location)
          .ThenInclude(x => x.LinkTaxonomies!.Where(lt => lt.LinkType == LinkType.Location))
          .ThenInclude(x => x.Taxonomy)

          .Include(x => x.ServiceAtLocations)
          .ThenInclude(x => x.RegularSchedules)

          .Include(x => x.ServiceAtLocations)
          .ThenInclude(x => x.HolidaySchedules)

          .Include(x => x.RegularSchedules)
          .Include(x => x.HolidaySchedules)

          .Include(x => x.CostOptions)
          .Where(x => x.Status == request.Status && x.Status != "Deleted" && organisationIds.Contains(x.OrganisationId));
        }
        else
        {
            services = _context.Services
           .Include(x => x.ServiceType)
           .Include(x => x.ServiceDeliveries)
           .Include(x => x.Eligibilities)
           .Include(x => x.LinkContacts)
           .Include(x => x.Languages)
           .Include(x => x.ServiceAreas)
           .Include(x => x.ServiceTaxonomies)
           .ThenInclude(x => x.Taxonomy)
           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.PhysicalAddresses)

           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.LinkTaxonomies!.Where(lt => lt.LinkType == LinkType.Location))
           .ThenInclude(x => x.Taxonomy)

           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.RegularSchedules)
           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.HolidaySchedules)
           .Include(x => x.RegularSchedules)
           .Include(x => x.HolidaySchedules)

           .Include(x => x.CostOptions)
           .Where(x => x.Status == request.Status && x.Status != "Deleted");
        }

        if (request.ServiceType != null)
        {
            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.ServiceType || x.Name == request.ServiceType);
            if (serviceType != null)
            {
                services = services.Where(x => x.ServiceType.Id == serviceType.Id);
            }
        }
        
        return services;
    }

}
