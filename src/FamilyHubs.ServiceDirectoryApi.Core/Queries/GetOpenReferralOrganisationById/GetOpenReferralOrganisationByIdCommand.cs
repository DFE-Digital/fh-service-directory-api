using Ardalis.GuardClauses;
using AutoMapper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.core.RecordEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.core.Queries.GetOpenReferralOrganisationById;


public class GetOpenReferralOrganisationByIdCommand : IRequest<OpenReferralOrganisationWithServicesRecord>
{
    public string Id { get; set; } = default!;
}

public class GetOpenReferralOrganisationByIdHandler : IRequestHandler<GetOpenReferralOrganisationByIdCommand, OpenReferralOrganisationWithServicesRecord>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOpenReferralOrganisationByIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<OpenReferralOrganisationWithServicesRecord> Handle(GetOpenReferralOrganisationByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OpenReferralOrganisations
           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceDelivery)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Eligibilitys)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Contacts)
           .ThenInclude(x => x.Phones)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Languages)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_areas)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_taxonomys)
           .ThenInclude(x => x.Taxonomy)
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralOrganisation), request.Id);
        }

        List<OpenReferralServiceRecord> openReferralServices = new();
        if (entity.Services != null)
        {
            foreach (OpenReferralService openReferralService in entity.Services)
            {
                openReferralServices.Add(new OpenReferralServiceRecord(
                    openReferralService.Id,
                    openReferralService.Name,
                    openReferralService.Description,
                    openReferralService.Accreditations,
                    openReferralService.Assured_date,
                    openReferralService.Attending_access,
                    openReferralService.Attending_type,
                    openReferralService.Deliverable_type,
                    openReferralService.Status,
                    openReferralService.Url,
                    openReferralService.Email,
                    openReferralService.Fees,
                    openReferralService.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryRecord(x.Id, x.ServiceDelivery)).ToList(),
                    openReferralService.Eligibilitys.Select(x => new OpenReferralEligibilityRecord(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList(),
                    openReferralService.Contacts.Select(x => new OpenReferralContactRecord(x.Id, x.Title, x.Name, x.Phones?.Select(x => new OpenReferralPhoneRecord(x.Id, x.Number)).ToList())).ToList(),
                    openReferralService.Cost_options.Select(x => new OpenReferralCost_OptionRecord(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList(),
                    openReferralService.Languages.Select(x => new OpenReferralLanguageRecord(x.Id, x.Language)).ToList(),
                    openReferralService.Service_areas.Select(x => new OpenReferralService_AreaRecord(x.Id, x.Service_area, x.Extent, x.Uri)).ToList(),
                    openReferralService.Service_at_locations.Select(x => new OpenReferralServiceAtLocationRecord(x.Id, new OpenReferralLocationRecord(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, x.Location?.Physical_addresses?.Select(x => new OpenReferralPhysical_AddressRecord(x.Id, x.Address_1, x.City, x.Postal_code, x.Country, x.State_province)).ToList()))).ToList(),
                    openReferralService.Service_taxonomys.Select(x => new OpenReferralService_TaxonomyRecord(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyRecord(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList()
                    ));
            }
        }


        var result = new OpenReferralOrganisationWithServicesRecord(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Logo,
            entity.Uri,
            entity.Url,
            openReferralServices);

        return result;
    }
}


