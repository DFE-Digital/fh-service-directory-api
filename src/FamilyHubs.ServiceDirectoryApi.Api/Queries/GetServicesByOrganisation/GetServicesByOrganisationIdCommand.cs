using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;

public class GetServicesByOrganisationIdCommand : IRequest<List<ServiceDto>>
{
    public GetServicesByOrganisationIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

public class GetServicesByOrganisationIdCommandHandler : IRequestHandler<GetServicesByOrganisationIdCommand, List<ServiceDto>>
{
    private readonly ApplicationDbContext _context;

    public GetServicesByOrganisationIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceDto>> Handle(GetServicesByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        var organisation = _context.Organisations
            .Include(x => x.OrganisationType)
            .Include(x => x.Services!.Where(s => s.Status != "Deleted"))
            .FirstOrDefault(x => x.Id == request.Id);

        if (organisation == null)
        {
            throw new NotFoundException(nameof(Service), request.Id);
        }

        var ids = organisation.Services?.Select(x => x.Id).ToList();

        if (ids == null || !ids.Any())
        {
            throw new NotFoundException(nameof(Service), request.Id);
        }

        var entity = await _context.Services
            .Include(x => x.ServiceType)
            .Include(x => x.ServiceDeliveries)
            .Include(x => x.Eligibilities)
            .Include(x => x.LinkContacts)
            .Include(x => x.CostOptions)
            .Include(x => x.Languages)
            .Include(x => x.ServiceAreas)
            
            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.PhysicalAddresses)

            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.LinkTaxonomies!)
            .ThenInclude(x => x.Taxonomy)

            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.RegularSchedules)

            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.ServiceTaxonomies)
            .ThenInclude(x => x.Taxonomy)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var result = DtoHelper.GetServicesDto(entity);

        return result;
    }
}

