using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetOpenReferralOrganisationById;


public class GetOpenReferralOrganisationByIdCommand : IRequest<OpenReferralOrganisationWithServicesDto>
{
    public string Id { get; set; } = default!;
}

public class GetOpenReferralOrganisationByIdHandler : IRequestHandler<GetOpenReferralOrganisationByIdCommand, OpenReferralOrganisationWithServicesDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOpenReferralOrganisationByIdHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<OpenReferralOrganisationWithServicesDto> Handle(GetOpenReferralOrganisationByIdCommand request, CancellationToken cancellationToken)
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

        List<OpenReferralServiceDto> openReferralServices = new();
        if (entity.Services != null)
        {
            foreach (OpenReferralService openReferralService in entity.Services)
            {

                openReferralServices.Add(OpenReferralDtoHelper.GetOpenReferralServiceDto(openReferralService));
                /*
                openReferralServices.Add(new OpenReferralServiceDto(
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
                    new List<IOpenReferralServiceDeliveryExDto>(openReferralService.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryExDto(x.Id, x.ServiceDelivery)).ToList()),
                    new List<IOpenReferralEligibilityDto>(openReferralService.Eligibilitys.Select(x => new OpenReferralEligibilityDto(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList()),
                    new List<IOpenReferralContactDto>(openReferralService.Contacts.Select(x => new OpenReferralContactDto(x.Id, x.Title, x.Name, new List<IOpenReferralPhoneDto>(x.Phones?.Select(x => new OpenReferralPhoneDto(x.Id, x.Number)).ToList()))).ToList()),
                    new List<IOpenReferralCostOptionDto>(openReferralService.Cost_options.Select(x => new OpenReferralCostOptionDto(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList()),
                    new List<IOpenReferralLanguageDto>(openReferralService.Languages.Select(x => new OpenReferralLanguageDto(x.Id, x.Language)).ToList()),
                    new List<IOpenReferralServiceAreaDto>(openReferralService.Service_areas.Select(x => new OpenReferralServiceAreaDto(x.Id, x.Service_area, x.Extent, x.Uri)).ToList()),
                    new List<IOpenReferralServiceAtLocationDto>(openReferralService.Service_at_locations.Select(x => new OpenReferralServiceAtLocationDto(x.Id, new OpenReferralLocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, new List<IOpenReferralPhysicalAddressDto>(x.Location?.Physical_addresses?.Select(x => new OpenReferralPhysicalAddressDto(x.Id, x.Address_1, x.City, x.Postal_code, x.Country, x.State_province)).ToList())))).ToList()),
                    new List<IOpenReferralServiceTaxonomyDto>(openReferralService.Service_taxonomys.Select(x => new OpenReferralServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList())
                    ));
                */
            }
        }


        var result = new OpenReferralOrganisationWithServicesDto(
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


