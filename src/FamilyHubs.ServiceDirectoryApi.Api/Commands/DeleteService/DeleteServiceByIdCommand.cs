using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
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
        try
        {
            var entity = await _context.Services
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                throw new NotFoundException(nameof(Service), request.Id.ToString());

            entity.Status = ServiceStatusType.Deleted;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw;
        }
    }
}
