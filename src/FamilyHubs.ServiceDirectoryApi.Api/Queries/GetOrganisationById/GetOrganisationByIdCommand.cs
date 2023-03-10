using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;


public class GetOrganisationByIdCommand : IRequest<OrganisationWithServicesDto>
{
    public long Id { get; set; }
}

public class GetOrganisationByIdHandler : IRequestHandler<GetOrganisationByIdCommand, OrganisationWithServicesDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrganisationByIdHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<OrganisationWithServicesDto> Handle(GetOrganisationByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Organisations
           .Include(x => x.Contacts)
           .Include(x => x.Services)
           .ThenInclude(x => x.ServiceDeliveries)
           .Include(x => x.Services)
           .ThenInclude(x => x.Eligibilities)
           .Include(x => x.Services)
           .ThenInclude(x => x.CostOptions)
           .Include(x => x.Services)
           .ThenInclude(x => x.Fundings)
           .Include(x => x.Services)
           .ThenInclude(x => x.Languages)
           .Include(x => x.Services)
           .ThenInclude(x => x.ServiceAreas)
           .Include(x => x.Services)
           .ThenInclude(x => x.RegularSchedules)
           .Include(x => x.Services)
           .ThenInclude(x => x.HolidaySchedules)
           .Include(x => x.Services)
           .ThenInclude(x => x.Contacts)
           .Include(x => x.Services)
           .ThenInclude(x => x.Taxonomies)
           
           .AsSplitQuery()

           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        var serviceIds = entity.Services.Select(x => x.Id).ToList();
        if (serviceIds.Any())
        {
            var serviceCollection = await _context.Services.Where(x => serviceIds.Any(y => y == x.Id))
                .Include(x => x.Locations)
                .ThenInclude(x => x.RegularSchedules)

                .Include(x => x.Locations)
                .ThenInclude(x => x.HolidaySchedules)

                .Include(x => x.Locations)
                .ThenInclude(x => x.Contacts)
                
                .AsSplitQuery()

                .ToListAsync(cancellationToken);

            entity.Services = serviceCollection;
        }

        var result = new OrganisationWithServicesDto
        {
            OrganisationType = OrganisationType.NotSet,
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            AdminAreaCode = entity.AdminAreaCode,
            Logo = entity.Logo,
            Uri = entity.Uri,
            Url = entity.Url,
            Contacts = _mapper.Map<List<ContactDto>>(entity.Contacts),
            Services = _mapper.Map<List<ServiceDto>>(entity.Services) 
        };

        return result;
    }
}


