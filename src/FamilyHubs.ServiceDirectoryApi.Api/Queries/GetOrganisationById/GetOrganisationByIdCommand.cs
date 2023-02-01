using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;


public class GetOrganisationByIdCommand : IRequest<OrganisationWithServicesDto>
{
    public string Id { get; set; } = default!;
}

public class GetOrganisationByIdHandler : IRequestHandler<GetOrganisationByIdCommand, OrganisationWithServicesDto>
{
    private readonly ApplicationDbContext _context;

    public GetOrganisationByIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OrganisationWithServicesDto> Handle(GetOrganisationByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Organisations
           .Include(x => x.OrganisationType)
           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceDeliveries)
           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceType)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Eligibilities)
           .Include(x => x.Services!)
           .ThenInclude(x => x.LinkContacts)
           .Include(x => x.Services!)
           .ThenInclude(x => x.CostOptions)
           .Include(x => x.Services!)
           .ThenInclude(x => x.Languages)
           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceAreas)

           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceAtLocations)
           .ThenInclude(x => x.RegularSchedules)

           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceAtLocations)
           .ThenInclude(x => x.HolidaySchedules)

           .Include(x => x.Services!)
           .ThenInclude(x => x.RegularSchedules)

           .Include(x => x.Services!)
           .ThenInclude(x => x.HolidaySchedules)

           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceAtLocations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.PhysicalAddresses)
           
           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceAtLocations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.LinkTaxonomies!)
           .ThenInclude(x => x.Taxonomy)

           .Include(x => x.Services!)
           .ThenInclude(x => x.ServiceTaxonomies)
           .ThenInclude(x => x.Taxonomy)
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Organisation), request.Id);
        }

        List<ServiceDto> services = new List<ServiceDto>();
        if (entity.Services != null)
        {
            foreach (var service in entity.Services)
            {
                services.Add(DtoHelper.GetServiceDto(service));
            }
        }

        var result = new OrganisationWithServicesDto(
            entity.Id,
            new OrganisationTypeDto(entity.OrganisationType.Id, entity.OrganisationType.Name, entity.OrganisationType.Description),
            entity.Name,
            entity.Description,
            entity.Logo,
            entity.Uri,
            entity.Url,
            services);

        var organisationAdminDistrict = _context.AdminAreas.FirstOrDefault(x => x.OrganisationId == entity.Id);
        if (organisationAdminDistrict != null)
        {
            result.AdminAreaCode = organisationAdminDistrict.Code;
        }
        
        return result;
    }
}


