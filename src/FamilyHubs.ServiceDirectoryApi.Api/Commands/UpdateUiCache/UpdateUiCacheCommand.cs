using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateUiCache;

public class UpdateUiCacheCommand : IRequest<string>
{
    public UpdateUiCacheCommand(string id, UICacheDto uiCacheDto)
    {
        Id = id;
        UiCacheDto = uiCacheDto;
    }

    public UICacheDto UiCacheDto { get; }

    public string Id { get; }
}

public class UpdateUiCacheCommandHandler : IRequestHandler<UpdateUiCacheCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateUiCacheCommandHandler> _logger;

    public UpdateUiCacheCommandHandler(ApplicationDbContext context, ILogger<UpdateUiCacheCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateUiCacheCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.UiCaches
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(UiCache), request.Id);
        }

        try
        {
            entity.Value = request.UiCacheDto.Value;
            
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating UICache. {exceptionMessage}", ex.Message);
            throw;
        }

        return entity.Id;
    }
}

