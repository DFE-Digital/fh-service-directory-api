using Ardalis.GuardClauses;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Commands.DeleteOpenReferralService;
public class DeleteOpenReferralServiceByIdCommand : IRequest<bool>
{
    public DeleteOpenReferralServiceByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; init; }
}

public class DeleteOpenReferralServiceByIdCommandHandler : IRequestHandler<DeleteOpenReferralServiceByIdCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DeleteOpenReferralServiceByIdCommandHandler> _logger;

    public DeleteOpenReferralServiceByIdCommandHandler(ApplicationDbContext context, ILogger<DeleteOpenReferralServiceByIdCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteOpenReferralServiceByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.OpenReferralServices
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(OpenReferralService), request.Id);
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
