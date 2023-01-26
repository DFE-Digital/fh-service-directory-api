using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateUICache;

public class UpdateUICacheCommand : IRequest<string>
{
    public UpdateUICacheCommand(string id, UICacheDto uiCacheDto)
    {
        Id = id;
        UICacheDto = uiCacheDto;
    }

    public UICacheDto UICacheDto { get; init; }

    public string Id { get; set; }
}

public class UpdateUICacheCommandHandler : IRequestHandler<UpdateUICacheCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateUICacheCommandHandler> _logger;

    public UpdateUICacheCommandHandler(ApplicationDbContext context, ILogger<UpdateUICacheCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateUICacheCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.UICaches
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(UICache), request.Id);
        }

        try
        {
            entity.Value = request.UICacheDto.Value;
            
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating UICache. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }
}

