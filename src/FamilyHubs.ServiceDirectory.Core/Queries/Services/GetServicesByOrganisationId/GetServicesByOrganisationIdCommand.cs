﻿using Ardalis.GuardClauses;
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

//todo: move somewhere common for reuse?
public enum SortOrder
{
    ascending,
    descending
}

public class GetServicesByOrganisationIdCommand : IRequest<PaginatedList<ServiceNameDto>>
{
    public required long Id { get; set; }
    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }
    public required SortOrder Order { get; set; }
}

public class GetServicesByOrganisationIdCommandHandler : IRequestHandler<GetServicesByOrganisationIdCommand, PaginatedList<ServiceNameDto>>
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
    public async Task<PaginatedList<ServiceNameDto>> Handle(GetServicesByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;

        //todo: do we need _context.ServiceNames?
        var servicesQuery = _context.Services
            .Where(s => s.Status != ServiceStatusType.Deleted && s.OrganisationId == request.Id);

        servicesQuery = request.Order == SortOrder.ascending
            ? servicesQuery.OrderBy(s => s.Name)
            : servicesQuery.OrderByDescending(s => s.Name);

        var services = await servicesQuery
            .Skip(skip)
            .Take(request.PageSize)

            .ProjectTo<ServiceNameDto>(_mapper.ConfigurationProvider)
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

        return new PaginatedList<ServiceNameDto>(services, totalCount, request.PageNumber, request.PageSize);
    }

}

