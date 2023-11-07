using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServicesByOrganisationId;

public class GetServicesByOrganisationIdCommand : IRequest<PaginatedList<ServiceDto>>
{
    public required long Id { get; set; }
    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }
}

public class GetServicesByOrganisationIdCommandHandler : IRequestHandler<GetServicesByOrganisationIdCommand, PaginatedList<ServiceDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetServicesByOrganisationIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //todo: only need to return name and id
    //todo: sort order (on name)
    //todo: pagination
    //public async Task<PaginatedList<ServiceDto>> Handle(GetServicesByOrganisationIdCommand request, CancellationToken cancellationToken)
    //{
    //    var services = await _context.Services
    //        //.Include(x => x.Taxonomies)

    //        //.Include(x => x.Locations)
    //        //.ThenInclude(x => x.Contacts)

    //        //.Include(x => x.Locations)
    //        //.ThenInclude(x => x.HolidaySchedules)

    //        //.Include(x => x.Locations)
    //        //.ThenInclude(x => x.RegularSchedules)

    //        .Where(s => s.Status != ServiceStatusType.Deleted)
    //        .Where(x => x.OrganisationId == request.Id)

    //        //todo: required?
    //        .AsSplitQuery()
    //        .AsNoTracking()

    //        .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)

    //        .ToListAsync(cancellationToken);

    //    //todo: client converts to empty list anyway, so best to return 404 or empty list?
    //    if (!services.Any())
    //        throw new NotFoundException(nameof(Service), request.Id.ToString());

    //    return new PaginatedList<ServiceDto>(services, services.Count, 1, services.Count);
    //}

    public async Task<PaginatedList<ServiceDto>> Handle(GetServicesByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;

        var services = await _context.Services
            .Where(s => s.Status != ServiceStatusType.Deleted && s.OrganisationId == request.Id)
            .OrderBy(s => s.Name)

            .Skip(skip)
            .Take(request.PageSize)

            .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        //todo: client converts to empty list anyway, so best to return 404 or empty list?
        if (!services.Any())
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        // get the total count of services
        int totalCount = await _context.Services
            .Where(s => s.Status != ServiceStatusType.Deleted)
            .Where(x => x.OrganisationId == request.Id)
            .CountAsync(cancellationToken);

        return new PaginatedList<ServiceDto>(services, totalCount, request.PageNumber, request.PageSize);
    }

}

