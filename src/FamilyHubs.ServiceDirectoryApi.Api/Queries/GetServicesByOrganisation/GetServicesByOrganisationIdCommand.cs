using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;

public class GetServicesByOrganisationIdCommand : IRequest<List<ServiceDto>>
{
    public GetServicesByOrganisationIdCommand(long id)
    {
        Id = id;
    }

    public long Id { get; }
}

public class GetServicesByOrganisationIdCommandHandler : IRequestHandler<GetServicesByOrganisationIdCommand, List<ServiceDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetServicesByOrganisationIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ServiceDto>> Handle(GetServicesByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        var organisation = _context.Organisations
            .Include(x => x.Services.Where(s => s.Status != ServiceStatusType.Deleted))
            .FirstOrDefault(x => x.Id == request.Id);

        if (organisation is null)
        {
            throw new NotFoundException(nameof(Service), request.Id.ToString());
        }

        var ids = organisation.Services.Select(x => x.Id).ToList();

        if (ids is null || !ids.Any())
        {
            throw new NotFoundException(nameof(Service), request.Id.ToString());
        }

        var entity = await _context.Services
            .Include(x => x.ServiceDeliveries)
            .Include(x => x.Eligibilities)
            .Include(x => x.CostOptions)
            .Include(x => x.Fundings)
            .Include(x => x.Languages)
            .Include(x => x.ServiceAreas)
            .Include(x => x.RegularSchedules)
            .Include(x => x.HolidaySchedules)
            .Include(x => x.Contacts)
            .Include(x => x.Taxonomies)

            .Include(x => x.Locations)
            .ThenInclude(x => x.RegularSchedules)
            
            .Include(x => x.Locations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)
            
            .Where(x => ids.Contains(x.Id))
            
            .AsSplitQuery()
            
            .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)
            
            .ToListAsync(cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        return entity;
    }
}

