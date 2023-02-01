using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateUICache;

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
            ArgumentNullException.ThrowIfNull(entity);

            entity.RegisterDomainEvent(new UICacheCreatedEvent(entity));

            _context.UICaches.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating UICache. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return request.UICacheDto.Id;
    }
}