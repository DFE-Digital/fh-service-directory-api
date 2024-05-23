using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.DeleteService;
public class DeleteServiceByIdCommand : IRequest<bool>
{
    public DeleteServiceByIdCommand(long id)
    {
        Id = id;
    }

    public long Id { get; }
}

public class DeleteServiceByIdCommandHandler : IRequestHandler<DeleteServiceByIdCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DeleteServiceByIdCommandHandler> _logger;

    public DeleteServiceByIdCommandHandler(ApplicationDbContext context, ILogger<DeleteServiceByIdCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteServiceByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Services
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        entity.Status = ServiceStatusType.Deleted;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
