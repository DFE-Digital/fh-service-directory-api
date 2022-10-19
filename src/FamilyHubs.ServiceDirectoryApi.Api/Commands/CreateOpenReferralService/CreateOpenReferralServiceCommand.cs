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
