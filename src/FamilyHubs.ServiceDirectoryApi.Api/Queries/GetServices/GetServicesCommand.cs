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
    public GetServicesCommand(string? serviceType, string? status, string? districtCode, int? minimumAge, int? maximumAge, int? givenAge, double? latitude, double? longitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonomyIds, string? languages, bool? canFamilyChooseLocation, bool? isFamilyHub, int? maxFamilyHubs)
    {
        ServiceType = serviceType;
        Status = status;
        DistrictCode = districtCode;
        MaximumAge = maximumAge;
        MinimumAge = minimumAge;
        GivenAge = givenAge;
        Latitude = latitude;
        Longitude = longitude;
        Meters = proximity;
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? 10;
        Text = text;
        ServiceDeliveries = serviceDeliveries;
        IsPaidFor = isPaidFor;
        TaxonomyIds = taxonomyIds;
        Languages = languages;
        CanFamilyChooseLocation = canFamilyChooseLocation;
        IsFamilyHub = isFamilyHub;
        MaxFamilyHubs = maxFamilyHubs;
    }

    public string? ServiceType { get; }
    public string? Status { get; set; }
    public string? DistrictCode { get; }
    public int? MaximumAge { get; }
    public int? MinimumAge { get; }
    public int? GivenAge { get; }
    public double? Latitude { get; }
    public double? Longitude { get; }
    public double? Meters { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public string? Text { get; }
    public string? ServiceDeliveries { get; }
    public bool? IsPaidFor { get; }
    public string? TaxonomyIds { get; }
    public string? Languages { get; }
    public bool? CanFamilyChooseLocation { get; }
    public bool? IsFamilyHub { get; }
    public int? MaxFamilyHubs { get; }
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

        var entities = await GetServices(request);

        if (request.MaximumAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(eligibility => eligibility.MaximumAge <= request.MaximumAge.Value));

        if (request.MinimumAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(eligibility => eligibility.MinimumAge >= request.MinimumAge.Value));

        if (request.GivenAge != null)
            entities = entities.Where(x => x.Eligibilities.Any(eligibility => eligibility.MinimumAge <= request.GivenAge.Value && eligibility.MaximumAge >= request.GivenAge.Value));

        if (!string.IsNullOrEmpty(request.Text))
            entities = entities.Where(x => x.Name.Contains(request.Text) || x.Description != null && x.Description.Contains(request.Text));

        var dbServices = await entities.ToListAsync(cancellationToken);
        if (request.Latitude is not null && request.Longitude is not null && request.Meters is not null)
            dbServices = dbServices.Where(x => Core.Helper.GetDistance(request.Latitude, request.Longitude, x.ServiceAtLocations.FirstOrDefault()?.Location.Latitude, x.ServiceAtLocations.FirstOrDefault()?.Location.Longitude, x.Name) < request.Meters).ToList();

        //ServiceDeliveries
        if (!string.IsNullOrEmpty(request.ServiceDeliveries))
        {
            var servicesFilteredByDelMethod = new List<Service>();
            var parts = request.ServiceDeliveries.Split(',');
            
            foreach (var part in parts)
                if (Enum.TryParse(part, true, out ServiceDeliveryType serviceDelivery))
                    servicesFilteredByDelMethod.AddRange(dbServices.Where(x => x.ServiceDeliveries.Any(delivery => delivery.Name == serviceDelivery)).ToList());
            
            dbServices = servicesFilteredByDelMethod;
        }

        //Languages
        if (!string.IsNullOrEmpty(request.Languages))
        {
            var servicesFilteredByLanguages = new List<Service>();
            var parts = request.Languages.Split(',');
            
            foreach (var part in parts)
                servicesFilteredByLanguages.AddRange(dbServices.Where(x => x.Languages.Any(language => language.Name == part)).ToList());
            
            dbServices = servicesFilteredByLanguages;
        }

        if (request.IsPaidFor != null)
        {
            dbServices = dbServices.Where(x => IsPaidFor(x) == request.IsPaidFor).ToList();
        }

        //Can families choose location
        if (request.CanFamilyChooseLocation is true)
        {
            dbServices = dbServices.Where(x => x.CanFamilyChooseDeliveryLocation).ToList();
        }

        if (!string.IsNullOrEmpty(request.TaxonomyIds))
        {
            var parts = request.TaxonomyIds.Split(',');
            dbServices = dbServices.Where(x => x.ServiceTaxonomies.Any(serviceTaxonomy => parts.Contains(serviceTaxonomy.Taxonomy?.Id))).ToList();
        }

        // filter before we calculate distance and map, for efficiency
        if (request.IsFamilyHub != null)
        {
            dbServices = dbServices.Where(s =>
                s.ServiceAtLocations.FirstOrDefault()?.Location.LinkTaxonomies
                    ?.Any(lt => string.Equals(lt.Taxonomy?.Id, TaxonomyDtoIds.FamilyHub, StringComparison.OrdinalIgnoreCase)) == request.IsFamilyHub).ToList();
        }

        var filteredServices = DtoHelper.GetServicesDto(dbServices);
        if (request.Latitude is not null && request.Longitude is not null)
        {
            foreach (var service in filteredServices)
            {
                service.Distance = Core.Helper.GetDistance(
                    request.Latitude,
                    request.Longitude,
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

        var pageList = filteredServices.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
        var result = new PaginatedList<ServiceDto>(pageList, filteredServices.Count, request.PageNumber, request.PageSize);
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
            var organisationIds = await _context.AdminAreas.Where(x => x.Code == request.DistrictCode).Select(x => x.OrganisationId).ToListAsync();

           services = _context.Services
          .Include(x => x.ServiceType)
          .Include(x => x.ServiceDeliveries)
          .Include(x => x.Eligibilities)
          .Include(x => x.Languages)
          .Include(x => x.ServiceAreas)
          .Include(x => x.LinkContacts)
          .ThenInclude(x => x.Contact)
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
          .ThenInclude(x => x.Location)
          .ThenInclude(x => x.LinkContacts!)
          .ThenInclude(x => x.Contact)

          .Include(x => x.ServiceAtLocations)
          .Include(x => x.LinkContacts)
          .ThenInclude(x => x.Contact)

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
