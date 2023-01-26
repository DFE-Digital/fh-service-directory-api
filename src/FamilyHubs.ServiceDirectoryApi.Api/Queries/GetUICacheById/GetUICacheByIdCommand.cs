using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetUICacheById;

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


