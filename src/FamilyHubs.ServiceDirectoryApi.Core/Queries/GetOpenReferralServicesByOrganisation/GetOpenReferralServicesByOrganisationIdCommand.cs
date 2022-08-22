using Ardalis.GuardClauses;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.core.RecordEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.core.Queries.GetOpenReferralServicesByOrganisation;

public class GetOpenReferralServicesByOrganisationIdCommand : IRequest<List<OpenReferralServiceRecord>>
{
    public GetOpenReferralServicesByOrganisationIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public class GetOpenReferralServicesByOrganisationIdCommandHandler : IRequestHandler<GetOpenReferralServicesByOrganisationIdCommand, List<OpenReferralServiceRecord>>
{
    private readonly IApplicationDbContext _context;

    public GetOpenReferralServicesByOrganisationIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OpenReferralServiceRecord>> Handle(GetOpenReferralServicesByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        var organisation = _context.OpenReferralOrganisations
            .Include(x => x.Services)
            .FirstOrDefault(x => x.Id == request.Id);

        if (organisation == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        List<string>? ids = organisation?.Services?.Select(x => x.Id).ToList();

        if (ids == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        var entity = await _context.OpenReferralServices
            .Include(x => x.ServiceDelivery)
            .Include(x => x.Eligibilitys)
            .Include(x => x.Contacts)
            .ThenInclude(x => x.Phones)
            .Include(x => x.Languages)
            .Include(x => x.Service_areas)
            .Include(x => x.Service_at_locations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.Physical_addresses)
            .Include(x => x.Service_taxonomys)
            .ThenInclude(x => x.Taxonomy)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        var result = entity.Select(x => new OpenReferralServiceRecord(
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

        return result;
    }
}

