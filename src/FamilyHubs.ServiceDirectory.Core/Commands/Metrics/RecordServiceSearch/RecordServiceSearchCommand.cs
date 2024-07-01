using AutoMapper;
using FamilyHubs.ServiceDirectory.Data;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto.Metrics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Metrics.RecordServiceSearch;

public class RecordServiceSearchCommand : IRequest<long>
{
    public RecordServiceSearchCommand(ServiceSearchDto serviceSearch)
    {
        ServiceSearch = serviceSearch;
    }

    public ServiceSearchDto ServiceSearch { get; }
}

public class RecordServiceSearchCommandHandler : IRequestHandler<RecordServiceSearchCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<RecordServiceSearchCommandHandler> _logger;

    public RecordServiceSearchCommandHandler(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<RecordServiceSearchCommandHandler> logger
    )
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<long> Handle(
        RecordServiceSearchCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var orgId = request.ServiceSearch.OrganisationId;
            var code = request.ServiceSearch.DistrictCode;
            if (orgId == null && code != null)
            {
                var org = await _context.Organisations
                    .IgnoreAutoIncludes().AsNoTracking()
                    .FirstOrDefaultAsync(p => p.AdminAreaCode == code, cancellationToken);
                orgId = org?.Id;
            }

            // TODO: Check if given services exist, OR rely on FK constraint
            // from ServiceSearchResult.ServiceId -> Service.Id to disallow
            // saving non-existant services
            ServiceSearch serviceSearch = _mapper.Map<ServiceSearch>(request.ServiceSearch);
            serviceSearch.OrganisationId = orgId;

            serviceSearch.SearchTriggerEvent = null!; // Don't insert SearchTriggerEvent
            serviceSearch.ServiceSearchType = null!; // Don't insert ServiceSearchType

            _context.ServiceSearches.Add(serviceSearch);
            
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Service search with ID {ServiceSearchId} saved to DB.", serviceSearch.Id);

            return serviceSearch.Id;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured recording service search.");
            throw;
        }
    }
}
