using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OrganisationType;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
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
           .Include(x => x.OrganisationType)
           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceDelivery)
           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceType)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Eligibilities)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Contacts)
           .ThenInclude(x => x.Phones)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Cost_options)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Languages)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_areas)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_at_locations)
           .ThenInclude(x => x.Regular_schedule)

           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_at_locations)
           .ThenInclude(x => x.HolidayScheduleCollection)

           .Include(x => x.Services!)
           .ThenInclude(x => x.Regular_schedules)

           .Include(x => x.Services!)
           .ThenInclude(x => x.Holiday_schedules)

           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_at_locations)
           .ThenInclude(x => x.Regular_schedule)

           .Include(x => x.Services!)
           .ThenInclude(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.Physical_addresses)
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
            }
        }

        var result = new OpenReferralOrganisationWithServicesDto(
            entity.Id,
            new OrganisationTypeDto(entity.OrganisationType.Id, entity.OrganisationType.Name, entity.OrganisationType.Description),
            entity.Name,
            entity.Description,
            entity.Logo,
            entity.Uri,
            entity.Url,
            openReferralServices);
        
        return result;
    }
}


