using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
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
            .Include(x => x.OrganisationType)
            .Include(x => x.Services!.Where(x => x.Status != "Deleted"))
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
            .Include(x => x.ServiceType)
            .Include(x => x.ServiceDelivery)
            .Include(x => x.Eligibilities)
            .Include(x => x.Contacts)
            .ThenInclude(x => x.Phones)
            .Include(x => x.Cost_options)
            .Include(x => x.Languages)
            .Include(x => x.Service_areas)
            
            .Include(x => x.Service_at_locations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.Physical_addresses)

            .Include(x => x.Service_at_locations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.LinkTaxonomies!)
            .ThenInclude(x => x.Taxonomy)

            .Include(x => x.Service_at_locations)
            .ThenInclude(x => x.Regular_schedule)

            .Include(x => x.Service_at_locations)
            .ThenInclude(x => x.HolidayScheduleCollection)

            .Include(x => x.Service_taxonomys)
            .ThenInclude(x => x.Taxonomy)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        var result = OpenReferralDtoHelper.GetOpenReferralServicesDto(entity);
        return result;
    }
}

