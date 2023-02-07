using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateUiCache;

public class CreateUiCacheCommand : IRequest<string>
{
    public CreateUiCacheCommand(UICacheDto uiCacheDto)
    {
        UiCacheDto = uiCacheDto;
    }

    public UICacheDto UiCacheDto { get; }
}

public class CreateUiCacheCommandHandler : IRequestHandler<CreateUiCacheCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreateUiCacheCommandHandler> _logger;
    public CreateUiCacheCommandHandler(ApplicationDbContext context, ILogger<CreateUiCacheCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(CreateUiCacheCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new UiCache(request.UiCacheDto.Id, request.UiCacheDto.Value);
            ArgumentNullException.ThrowIfNull(entity);

            entity.RegisterDomainEvent(new UiCacheCreatedEvent(entity));

            _context.UiCaches.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating UICache. {exceptionMessage}", ex.Message);
            throw;
        }

        return request.UiCacheDto.Id;
    }
}