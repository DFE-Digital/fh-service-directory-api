using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Commands.UpdateUICache;

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

    public UpdateUICacheCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(UpdateUICacheCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.UICaches
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

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
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }
}

