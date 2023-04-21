using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.UpdateTaxonomy;

public class UpdateTaxonomyCommand : IRequest<long>
{
    public UpdateTaxonomyCommand(long id, TaxonomyDto taxonomy)
    {
        Id = id;
        Taxonomy = taxonomy;
    }

    public TaxonomyDto Taxonomy { get; }

    public long Id { get; }
}

public class UpdateTaxonomyCommandHandler : IRequestHandler<UpdateTaxonomyCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateTaxonomyCommandHandler> _logger;

    public UpdateTaxonomyCommandHandler(ApplicationDbContext context, ILogger<UpdateTaxonomyCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<long> Handle(UpdateTaxonomyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.Taxonomies
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Taxonomy), request.Id.ToString());

        try
        {
            entity.Name = request.Taxonomy.Name;
            entity.TaxonomyType = request.Taxonomy.TaxonomyType;
            entity.ParentId = request.Taxonomy.ParentId;

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating taxonomy. {exceptionMessage}", ex.Message);
            throw;
        }

        return entity.Id;
    }
}


