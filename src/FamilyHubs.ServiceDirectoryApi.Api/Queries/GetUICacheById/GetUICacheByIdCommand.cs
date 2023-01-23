using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetUICacheById;

public class GetUICacheByIdCommand : IRequest<UICacheDto>
{
    public GetUICacheByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; init; } = default!;
}

public class GetUICacheByIdCommandHandler : IRequestHandler<GetUICacheByIdCommand, UICacheDto>
{
    private readonly ApplicationDbContext _context;

    public GetUICacheByIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<UICacheDto> Handle(GetUICacheByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UICaches
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(UICache), request.Id);
        }

        var result = new UICacheDto(entity.Id, entity.Value);
        

        return result;
    }
}


