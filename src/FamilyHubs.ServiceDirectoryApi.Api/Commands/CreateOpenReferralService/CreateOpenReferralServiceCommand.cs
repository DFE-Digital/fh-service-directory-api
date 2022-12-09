using Autofac.Core;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Commands.CreateOpenReferralService;

public class CreateOpenReferralServiceCommand : IRequest<string>
{
    public CreateOpenReferralServiceCommand(OpenReferralServiceDto openReferralService)
    {
        OpenReferralService = openReferralService;
    }

    public OpenReferralServiceDto OpenReferralService { get; init; }
}

public class CreateOpenReferralServiceCommandHandler : IRequestHandler<CreateOpenReferralServiceCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOpenReferralServiceCommandHandler> _logger;

    public CreateOpenReferralServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateOpenReferralServiceCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<OpenReferralService>(request.OpenReferralService);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.OpenReferralService.ServiceType.Id);
            if (serviceType != null)
                entity.ServiceType = serviceType;

            if (entity.Service_taxonomys != null)
            {
                foreach(var servicetaxonomy in entity.Service_taxonomys)
                {
                    if (servicetaxonomy != null && servicetaxonomy.Taxonomy != null)
                    {
                        var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == servicetaxonomy.Taxonomy.Id);
                        if (taxonomy != null)
                        {
                            servicetaxonomy.Taxonomy = taxonomy;
                        }
                    }
                    
                }
            }

            foreach (var serviceAtLocation in entity.Service_at_locations)
            {
                if (serviceAtLocation.Regular_schedule != null)
                {
                    foreach (var regularSchedules in serviceAtLocation.Regular_schedule)
                    {
                        regularSchedules.OpenReferralServiceAtLocationId = serviceAtLocation.Id;
                    }
                }

                if (serviceAtLocation.HolidayScheduleCollection != null)
                {
                    foreach (var holidaySchedules in serviceAtLocation.HolidayScheduleCollection)
                    {
                        holidaySchedules.OpenReferralServiceAtLocationId = serviceAtLocation.Id;
                    }
                }
            }

            entity.RegisterDomainEvent(new OpenReferralServiceCreatedEvent(entity));

            _context.OpenReferralServices.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating taxonomy. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        if (request is not null && request.OpenReferralService is not null)
            return request.OpenReferralService.Id;
        else
            return string.Empty;
    }
}
