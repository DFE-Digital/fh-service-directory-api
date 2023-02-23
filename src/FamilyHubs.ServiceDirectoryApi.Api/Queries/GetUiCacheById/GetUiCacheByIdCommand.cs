using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetUiCacheById;

public class GetUiCacheByIdCommand : IRequest<UICacheDto>
{
    public GetUiCacheByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

public class GetUiCacheByIdCommandHandler : IRequestHandler<GetUiCacheByIdCommand, UICacheDto>
{
    private readonly ApplicationDbContext _context;

    public GetUiCacheByIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<UICacheDto> Handle(GetUiCacheByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UiCaches
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(UiCache), request.Id);
        }

        var result = new UICacheDto(entity.Id, entity.Value);
        

        return result;
    }
}


