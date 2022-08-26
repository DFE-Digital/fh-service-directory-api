using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhones;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetOpenReferralServicesByOrganisation;

public class GetOpenReferralServicesByOrganisationIdCommand : IRequest<List<OpenReferralServiceDto>>
{
    public GetOpenReferralServicesByOrganisationIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public class GetOpenReferralServicesByOrganisationIdCommandHandler : IRequestHandler<GetOpenReferralServicesByOrganisationIdCommand, List<OpenReferralServiceDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOpenReferralServicesByOrganisationIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OpenReferralServiceDto>> Handle(GetOpenReferralServicesByOrganisationIdCommand request, CancellationToken cancellationToken)
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

        var result = entity.Select(x => new OpenReferralServiceDto(
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
            new List<IOpenReferralServiceDeliveryExDto>(x.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryExDto(x.Id, x.ServiceDelivery)).ToList()),
            new List<IOpenReferralEligibilityDto>(x.Eligibilitys.Select(x => new OpenReferralEligibilityDto(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList()),
            new List<IOpenReferralContactDto>(x.Contacts.Select(x => new OpenReferralContactDto(x.Id, x.Title, x.Name, new List<IOpenReferralPhoneDto>(x.Phones?.Select(x => new OpenReferralPhoneDto(x.Id, x.Number)).ToList()))).ToList()),
            new List<IOpenReferralCostOptionDto>(x.Cost_options.Select(x => new OpenReferralCostOptionDto(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList()),
            new List<IOpenReferralLanguageDto>(x.Languages.Select(x => new OpenReferralLanguageDto(x.Id, x.Language)).ToList()),
            new List<IOpenReferralServiceAreaDto>(x.Service_areas.Select(x => new OpenReferralServiceAreaDto(x.Id, x.Service_area, x.Extent, x.Uri)).ToList()),
            new List<IOpenReferralServiceAtLocationDto>(x.Service_at_locations.Select(x => new OpenReferralServiceAtLocationDto(x.Id, new OpenReferralLocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, new List<IOpenReferralPhysicalAddressDto>(x.Location?.Physical_addresses?.Select(x => new OpenReferralPhysicalAddressDto(x.Id, x.Address_1, x.City, x.Postal_code, x.Country, x.State_province)).ToList())))).ToList()),
            new List<IOpenReferralServiceTaxonomyDto>(x.Service_taxonomys.Select(x => new OpenReferralServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList())
            )).ToList();

        return result;
    }
}

