using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetOpenReferralService;

public class GetOpenReferralServiceByIdCommand : IRequest<OpenReferralServiceDto>
{
    public GetOpenReferralServiceByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public class GetOpenReferralServiceByIdCommandHandler : IRequestHandler<GetOpenReferralServiceByIdCommand, OpenReferralServiceDto>
{
    private readonly ApplicationDbContext _context;

    public GetOpenReferralServiceByIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OpenReferralServiceDto> Handle(GetOpenReferralServiceByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OpenReferralServices
            .Include(x => x.ServiceType)
            .Include(x => x.ServiceDelivery)
            .Include(x => x.Eligibilities)
            .Include(x => x.Contacts)
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

            .Include(x => x.Regular_schedules)
            .Include(x => x.Holiday_schedules)
            .Include(x => x.Service_taxonomys)
            .ThenInclude(x => x.Taxonomy)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        var result = OpenReferralDtoHelper.GetOpenReferralServiceDto(entity);
        return result;
    }
}

