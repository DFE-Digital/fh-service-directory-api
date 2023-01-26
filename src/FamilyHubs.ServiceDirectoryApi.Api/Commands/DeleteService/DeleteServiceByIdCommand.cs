using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
public class DeleteServiceByIdCommand : IRequest<bool>
{
    public DeleteServiceByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; init; }
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

            if (entity == null)
            {
                throw new NotFoundException(nameof(Service), request.Id);
            }

            entity.Status = "Deleted";

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }
    }
}
