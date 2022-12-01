using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.SharedKernel;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetServices;

public class GetOpenReferralServicesCommand : IRequest<PaginatedList<OpenReferralServiceDto>>
{
    public GetOpenReferralServicesCommand() { }
    public GetOpenReferralServicesCommand(string? serviceType, string? status, string? districtCode, int? minimum_age, int? maximum_age, int? given_age, double? latitude, double? longtitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonmyIds, string? languages, bool? canFamilyChooseLocation)
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
        PageSize = pageSize != null ? pageSize.Value : 1;
        Text = text;
        ServiceDeliveries = serviceDeliveries;
        IsPaidFor = isPaidFor;
        TaxonmyIds = taxonmyIds;
        Languages = languages;
        CanFamilyChooseLocation = canFamilyChooseLocation;
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

}

public class GetOpenReferralServicesCommandHandler : IRequestHandler<GetOpenReferralServicesCommand, PaginatedList<OpenReferralServiceDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOpenReferralServicesCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<OpenReferralServiceDto>> Handle(GetOpenReferralServicesCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Status))
            request.Status = "active";

        IQueryable<OpenReferralService> entities = await GetOpenReferralServices(request);

        if (request?.MaximumAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(x => x.Maximum_age <= request.MaximumAge.Value));

        if (request?.MinimumAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(x => x.Minimum_age >= request.MinimumAge.Value));

        if (request?.GivenAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(x => x.Minimum_age <= request.GivenAge.Value && x.Maximum_age >= request.GivenAge.Value));

        if (request?.Text != null)
            entities = entities.Where(x => x.Name.Contains(request.Text) || x.Description != null && x.Description.Contains(request.Text));


        IEnumerable<OpenReferralService> dbservices = await entities.ToListAsync();
        if (request?.Latitude != null && request?.Longtitude != null && request?.Meters != null)
            dbservices = dbservices.Where(x => core.Helper.GetDistance(request.Latitude, request.Longtitude, x?.Service_at_locations?.FirstOrDefault()?.Location.Latitude, x?.Service_at_locations?.FirstOrDefault()?.Location.Longitude, x?.Name) < request.Meters);

        
        //ServiceDeliveries
        if (request?.ServiceDeliveries != null)
        {
            List<OpenReferralService> servicesFilteredByDelMethod = new List<OpenReferralService>();
            string[] parts = request.ServiceDeliveries.Split(',');
            
            foreach (string part in parts)
                if (Enum.TryParse(part, true, out ServiceDelivery serviceDelivery))
                    servicesFilteredByDelMethod.AddRange(dbservices.Where(x => x.ServiceDelivery.Any(x => x.ServiceDelivery == serviceDelivery)).ToList());
            
            dbservices = servicesFilteredByDelMethod;
        }

        //Languages
        if (request?.Languages != null)
        {
            List<OpenReferralService> servicesFilteredByLanguages = new List<OpenReferralService>();
            string[] parts = request.Languages.Split(',');
            
            foreach (string part in parts)
                servicesFilteredByLanguages.AddRange(dbservices.Where(x => x.Languages.Any(x => x.Language == part)).ToList());
            
            dbservices = servicesFilteredByLanguages;
        }

        if (request?.IsPaidFor != null)
        {
            if (request?.IsPaidFor.Value == true)
            {
                dbservices = dbservices.Where(x => x.Cost_options.Any() == true);
            }
            if (request?.IsPaidFor.Value == false)
            {
                dbservices = dbservices.Where(x => x.Cost_options.Any() == false);
            }
        }

        //Can families choose location
        if (request?.CanFamilyChooseLocation != null)
        {
            if (request?.CanFamilyChooseLocation.Value == true)
            {
                dbservices = dbservices.Where(x => x.CanFamilyChooseDeliveryLocation == true);
            }
        }

        if (request?.TaxonmyIds != null)
        {
            string[] parts = request.TaxonmyIds.Split(',');
            dbservices = dbservices.Where(x => x.Service_taxonomys.Any(x => parts.Contains(x.Taxonomy?.Id) ));

        }

        if (dbservices == null)
        {
            dbservices = entities.ToList();
            if (dbservices == null)
                dbservices = new List<OpenReferralService>();
        }

        //if (request?.Latitude != null && request?.Longtitude != null)
        //    dbservices = dbservices.OrderBy(x => core.Helper.GetDistance(
        //        request.Latitude,
        //        request.Longtitude,
        //        x?.Service_at_locations?.FirstOrDefault()?.Location.Latitude,
        //        x?.Service_at_locations?.FirstOrDefault()?.Location.Longitude,
        //        x?.Name));

        var filteredServices = OpenReferralDtoHelper.GetOpenReferralServicesDto(dbservices);
        if (request?.Latitude != null && request?.Longtitude != null)
        {
            foreach (var service in filteredServices)
            {
                service.Distance = core.Helper.GetDistance(
                    request.Latitude,
                    request.Longtitude,
                    service.Service_at_locations?.FirstOrDefault()?.Location.Latitude,
                    service.Service_at_locations?.FirstOrDefault()?.Location.Longitude);
            }

            filteredServices = filteredServices.OrderBy(x => x.Distance).ToList();
        }
        

        if (request != null)
        {
            var pagelist = filteredServices.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<OpenReferralServiceDto>(filteredServices, pagelist.Count, request.PageNumber, request.PageSize);
            return result;
        }

        return new PaginatedList<OpenReferralServiceDto>(filteredServices, filteredServices.Count, 1, 10);

    }

    
    private async Task<IQueryable<OpenReferralService>> GetOpenReferralServices(GetOpenReferralServicesCommand request)
    {
        IQueryable<OpenReferralService> openReferralServices;

        if (request.DistrictCode != null)
        {
            List<string> organisationIds = await _context.OrganisationAdminDistricts.Where(x => x.Code == request.DistrictCode).Select(x => x.OpenReferralOrganisationId).ToListAsync();

           openReferralServices = _context.OpenReferralServices
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

          .Include(x => x.Service_at_locations)
          .ThenInclude(x => x.Regular_schedule)
          .Include(x => x.Service_at_locations)
          .ThenInclude(x => x.HolidayScheduleCollection)
          .Include(x => x.Regular_schedules)
          .Include(x => x.Holiday_schedules)

          .Include(x => x.Cost_options)
          .Where(x => x.Status == request.Status && x.Status != "Deleted" && organisationIds.Contains(x.OpenReferralOrganisationId));
        }
        else
        {
            openReferralServices = _context.OpenReferralServices
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
           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Regular_schedule)
           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.HolidayScheduleCollection)
           .Include(x => x.Regular_schedules)
           .Include(x => x.Holiday_schedules)
           .Include(x => x.Cost_options)
           .Where(x => x.Status == request.Status && x.Status != "Deleted");
        }

        if (request.ServiceType != null)
        {
            ServiceType? serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.ServiceType || x.Name == request.ServiceType);
            if (serviceType != null)
            {
                openReferralServices = openReferralServices.Where(x => x.ServiceType.Id == serviceType.Id);
            }
        }
        
        return openReferralServices;
    }

}
