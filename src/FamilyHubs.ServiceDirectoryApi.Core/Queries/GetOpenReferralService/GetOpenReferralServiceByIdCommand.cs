using Ardalis.GuardClauses;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.core.RecordEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace fh_service_directory_api.core.Commands.GetOpenReferralService;

public class GetOpenReferralServiceByIdCommand : IRequest<OpenReferralServiceRecord>
{
    public GetOpenReferralServiceByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public class GetOpenReferralServiceByIdCommandHandler : IRequestHandler<GetOpenReferralServiceByIdCommand, OpenReferralServiceRecord>
{
    private readonly IApplicationDbContext _context;

    public GetOpenReferralServiceByIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OpenReferralServiceRecord> Handle(GetOpenReferralServiceByIdCommand request, CancellationToken cancellationToken)
    {
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
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        var result = new OpenReferralServiceRecord(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Accreditations,
            entity.Assured_date,
            entity.Attending_access,
            entity.Attending_type,
            entity.Deliverable_type,
            entity.Status,
            entity.Url,
            entity.Email,
            entity.Fees,
            entity.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryRecord(x.Id, x.ServiceDelivery)).ToList(),
            entity.Eligibilitys.Select(x => new OpenReferralEligibilityRecord(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList(),
            entity.Contacts.Select(x => new OpenReferralContactRecord(x.Id, x.Title, x.Name, x.Phones?.Select(x => new OpenReferralPhoneRecord(x.Id, x.Number)).ToList())).ToList(),
            entity.Cost_options.Select(x => new OpenReferralCost_OptionRecord(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList(),
            entity.Languages.Select(x => new OpenReferralLanguageRecord(x.Id, x.Language)).ToList(),
            entity.Service_areas.Select(x => new OpenReferralService_AreaRecord(x.Id, x.Service_area, x.Extent, x.Uri)).ToList(),
            entity.Service_at_locations.Select(x => new OpenReferralServiceAtLocationRecord(x.Id, new OpenReferralLocationRecord(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, x.Location?.Physical_addresses?.Select(x => new OpenReferralPhysical_AddressRecord(x.Id, x.Address_1, x.City, x.Postal_code, x.Country, x.State_province)).ToList()))).ToList(),
            entity.Service_taxonomys.Select(x => new OpenReferralService_TaxonomyRecord(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyRecord(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList()
            );

        return result;
    }
}

