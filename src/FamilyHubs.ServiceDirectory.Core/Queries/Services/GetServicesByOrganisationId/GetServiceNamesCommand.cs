using AutoMapper;
using AutoMapper.QueryableExtensions;
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

//todo: rename ServiceNameDto to ServiceSummaryDto?
public class GetServiceNamesCommand : IRequest<PaginatedList<ServiceNameDto>>
{
    public long? OrganisationId { get; set; }
    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }
    public required SortOrder Order { get; set; }
}

public class GetServiceNamesCommandHandler : IRequestHandler<GetServiceNamesCommand, PaginatedList<ServiceNameDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetServiceNamesCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ServiceNameDto>> Handle(GetServiceNamesCommand request, CancellationToken cancellationToken)
    {
        var services = await GetServiceNames(request, cancellationToken);

        // NotFoundException is for when a queried object by a particular key is null,
        // i.e. searching by org, but the org doesn't exists, not when there are no services
        //todo: we could add a check to see if the organisation doesn't exist and throw NotFoundException
        //if (request.Id.HasValue && !services.Any())
        //    throw new NotFoundException(nameof(Service), request.Id.ToString());

        // get the total count of services
        int totalCount = await GetServicesCount(request, cancellationToken);

        return new PaginatedList<ServiceNameDto>(services, totalCount, request.PageNumber, request.PageSize);
    }

    private async Task<List<ServiceNameDto>> GetServiceNames(GetServiceNamesCommand request, CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;

        //todo: do we need _context.ServiceNames?
        var servicesQuery = _context.Services
            .Where(s => s.Status != ServiceStatusType.Deleted);

        if (request.OrganisationId != null)
        {
            servicesQuery = servicesQuery.Where(s => s.OrganisationId == request.OrganisationId);
        }

        servicesQuery = request.Order == SortOrder.ascending
            ? servicesQuery.OrderBy(s => s.Name)
            : servicesQuery.OrderByDescending(s => s.Name);

        return await servicesQuery
            .Skip(skip)
            .Take(request.PageSize)
            .ProjectTo<ServiceNameDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    private async Task<int> GetServicesCount(GetServiceNamesCommand request, CancellationToken cancellationToken)
    {
        var serviceCountQuery = _context.Services
            .Where(s => s.Status != ServiceStatusType.Deleted);

        if (request.OrganisationId != null)
        {
            serviceCountQuery = serviceCountQuery.Where(s => s.OrganisationId == request.OrganisationId);
        }

        return await serviceCountQuery.CountAsync(cancellationToken);
    }
}
