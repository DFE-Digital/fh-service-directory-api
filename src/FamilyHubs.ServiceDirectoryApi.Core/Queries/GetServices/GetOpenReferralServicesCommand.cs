using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.core.RecordEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.core.Queries.GetServices;

public class GetOpenReferralServicesCommand : IRequest<PaginatedList<OpenReferralServiceRecord>>
{
    public GetOpenReferralServicesCommand() { }
    public GetOpenReferralServicesCommand(string? status, int? minimum_age, int? maximum_age, double? latitude, double? longtitude, double? proximity, int? pageNumber, int? pageSize, string? text)
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

}

public class GetOpenReferralServicesCommandHandler : IRequestHandler<GetOpenReferralServicesCommand, PaginatedList<OpenReferralServiceRecord>>
{
    private readonly IApplicationDbContext _context;

    public GetOpenReferralServicesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<OpenReferralServiceRecord>> Handle(GetOpenReferralServicesCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Status))
            request.Status = "active";

        var entities = await _context.OpenReferralServices
           .Include(x => x.ServiceDelivery)
           .Include(x => x.Eligibilitys)
           .Include(x => x.Contacts)
           .ThenInclude(x => x.Phones)
           .Include(x => x.Languages)
           .Include(x => x.Service_areas)
           .Include(x => x.Service_taxonomys)
           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .Where(x => x.Status == request.Status).ToListAsync();

        IEnumerable<OpenReferralService> dbservices = default!;
        if (request?.Latitude != null && request?.Longtitude != null && request?.Meters != null)
            dbservices = entities.Where(x => Helper.GetDistance(request.Latitude, request.Longtitude, x?.Service_at_locations?.FirstOrDefault()?.Location.Latitude, x?.Service_at_locations?.FirstOrDefault()?.Location.Longitude, x?.Name) < request.Meters);

        if (request?.MaximumAge != null)
            dbservices = dbservices.Where(x => x.Eligibilitys.Any(x => x.Maximum_age <= request.MaximumAge.Value));

        if (request?.MinimumAge != null)
            dbservices = dbservices.Where(x => x.Eligibilitys.Any(x => x.Minimum_age >= request.MinimumAge.Value));

        if (request?.Text != null)
        {
            dbservices = dbservices.Where(x => x.Name.Contains(request.Text) || x.Description != null && x.Description.Contains(request.Text));
        }

        if (dbservices == null)
        {
            dbservices = new List<OpenReferralService>();
        }

        var filteredServices = dbservices.Select(x => new OpenReferralServiceRecord(
            x.Id,
            x.Name,
            x.Description,
            x.Accreditations,
            x.Assured_date,
            x.Attending_access,
            x.Attending_type,
            x.Deliverable_type,
            x.Status,
            x.Url,
            x.Email,
            x.Fees,
            x.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryRecord(x.Id, x.ServiceDelivery)).ToList(),
            x.Eligibilitys.Select(x => new OpenReferralEligibilityRecord(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList(),
            x.Contacts.Select(x => new OpenReferralContactRecord(x.Id, x.Title, x.Name, x.Phones?.Select(x => new OpenReferralPhoneRecord(x.Id, x.Number)).ToList())).ToList(),
            x.Cost_options.Select(x => new OpenReferralCost_OptionRecord(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList(),
            x.Languages.Select(x => new OpenReferralLanguageRecord(x.Id, x.Language)).ToList(),
            x.Service_areas.Select(x => new OpenReferralService_AreaRecord(x.Id, x.Service_area, x.Extent, x.Uri)).ToList(),
            x.Service_at_locations.Select(x => new OpenReferralServiceAtLocationRecord(x.Id, new OpenReferralLocationRecord(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, x.Location?.Physical_addresses?.Select(x => new OpenReferralPhysical_AddressRecord(x.Id, x.Address_1, x.City, x.Postal_code, x.Country, x.State_province)).ToList()))).ToList(),
            x.Service_taxonomys.Select(x => new OpenReferralService_TaxonomyRecord(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyRecord(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList()
            )).ToList();

        if (request != null)
        {
            var pagelist = filteredServices.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<OpenReferralServiceRecord>(filteredServices, pagelist.Count, request.PageNumber, request.PageSize);
            return result;
        }

        return new PaginatedList<OpenReferralServiceRecord>(filteredServices, filteredServices.Count, 1, 10);

    }

}
