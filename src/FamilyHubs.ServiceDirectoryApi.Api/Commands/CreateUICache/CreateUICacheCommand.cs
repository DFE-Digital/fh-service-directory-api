using FamilyHubs.ServiceDirectory.Shared.Models.Api;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Commands.CreateUICache;

public class CreateUICacheCommand : IRequest<string>
{
    public CreateUICacheCommand(UICacheDto uiCacheDto)
    {
        UICacheDto = uiCacheDto;
    }

    public UICacheDto UICacheDto { get; init; }
}

public class CreateUICacheCommandHandler : IRequestHandler<CreateUICacheCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreateUICacheCommandHandler> _logger;
    public CreateUICacheCommandHandler(ApplicationDbContext context, ILogger<CreateUICacheCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(CreateUICacheCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new UICache(request.UICacheDto.Id, request.UICacheDto.Value);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            entity.RegisterDomainEvent(new UICacheCreatedEvent(entity));

            _context.UICaches.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating UICache. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        if (request is not null && request.UICacheDto is not null)
            return request.UICacheDto.Id;
        return string.Empty;
    }
}