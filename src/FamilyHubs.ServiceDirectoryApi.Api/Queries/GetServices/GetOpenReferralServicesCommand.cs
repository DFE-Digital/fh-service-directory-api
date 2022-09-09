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
    public GetOpenReferralServicesCommand(string? status, int? minimum_age, int? maximum_age, double? latitude, double? longtitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, string? taxonmyIds)
    {
        Status = status;
        MaximumAge = maximum_age;
        MinimumAge = minimum_age;
        Latitude = latitude;
        Longtitude = longtitude;
        Meters = proximity;
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 1;
        Text = text;
        ServiceDeliveries = serviceDeliveries;
        TaxonmyIds = taxonmyIds;
    }

    public string? Status { get; set; }
    public int? MaximumAge { get; set; }
    public int? MinimumAge { get; set; }
    public double? Latitude { get; set; }
    public double? Longtitude { get; set; }
    public double? Meters { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Text { get; set; }
    public string? ServiceDeliveries { get; set; }
    public string? TaxonmyIds { get; set; }


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

        var entities = await _context.OpenReferralServices
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
           .Where(x => x.Status == request.Status).ToListAsync();

        IEnumerable<OpenReferralService> dbservices = entities;
        if (request?.Latitude != null && request?.Longtitude != null && request?.Meters != null)
            dbservices = entities.Where(x => fh_service_directory_api.core.Helper.GetDistance(request.Latitude, request.Longtitude, x?.Service_at_locations?.FirstOrDefault()?.Location.Latitude, x?.Service_at_locations?.FirstOrDefault()?.Location.Longitude, x?.Name) < request.Meters);

        if (request?.MaximumAge != null)
            dbservices = dbservices.Where(x => x.Eligibilities.Any(x => x.Maximum_age <= request.MaximumAge.Value));

        if (request?.MinimumAge != null)
            dbservices = dbservices.Where(x => x.Eligibilities.Any(x => x.Minimum_age >= request.MinimumAge.Value));

        if (request?.Text != null)
        {
            dbservices = dbservices.Where(x => x.Name.Contains(request.Text) || x.Description != null && x.Description.Contains(request.Text));
        }

        if (request?.ServiceDeliveries != null)
        {
            string[] parts = request.ServiceDeliveries.Split(',');
            foreach (string part in parts)
            {
                if (Enum.TryParse("part", out ServiceDelivery serviceDelivery))
                {
                    dbservices = dbservices.Where(x => x.ServiceDelivery.Any(x => x.ServiceDelivery == serviceDelivery));
                }   
            }
        }

        if (request?.TaxonmyIds != null)
        {
            string[] parts = request.TaxonmyIds.Split(',');
            foreach (string part in parts)
                dbservices = dbservices.Where(x => x.Service_taxonomys.Any(x => x.Taxonomy?.Id == part));

        }

        if (dbservices == null)
        {
            dbservices = entities.ToList();
            if (dbservices == null)
            dbservices = new List<OpenReferralService>();
        }

        var filteredServices = OpenReferralDtoHelper.GetOpenReferralServicesDto(dbservices);
        
        if (request != null)
        {
            var pagelist = filteredServices.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<OpenReferralServiceDto>(filteredServices, pagelist.Count, request.PageNumber, request.PageSize);
            return result;
        }

        return new PaginatedList<OpenReferralServiceDto>(filteredServices, filteredServices.Count, 1, 10);

    }

}
