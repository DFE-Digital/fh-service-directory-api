using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetService;

public class GetServiceByIdCommand : IRequest<ServiceDto>
{
    public GetServiceByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public class GetServiceByIdCommandHandler : IRequestHandler<GetServiceByIdCommand, ServiceDto>
{
    private readonly ApplicationDbContext _context;

    public GetServiceByIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceDto> Handle(GetServiceByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Services
            .Include(x => x.ServiceType)
            .Include(x => x.ServiceDeliveries)
            .Include(x => x.Eligibilities)
            .Include(x => x.Contacts)
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

            .Include(x => x.RegularSchedules)
            .Include(x => x.HolidaySchedules)
            .Include(x => x.ServiceTaxonomies)
            .ThenInclude(x => x.Taxonomy)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Service), request.Id);
        }

        var result = DtoHelper.GetServiceDto(entity);
        return result;
    }
}

