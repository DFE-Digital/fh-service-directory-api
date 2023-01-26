﻿using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateTaxonomy;

public class UpdateTaxonomyCommand : IRequest<string>
{
    public UpdateTaxonomyCommand(string id, TaxonomyDto taxonomy)
    {
        Id = id;
        Taxonomy = taxonomy;
    }

    public TaxonomyDto Taxonomy { get; init; }

    public string Id { get; set; }
}

public class UpdateTaxonomyCommandHandler : IRequestHandler<UpdateTaxonomyCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateTaxonomyCommandHandler> _logger;

    public UpdateTaxonomyCommandHandler(ApplicationDbContext context, ILogger<UpdateTaxonomyCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateTaxonomyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.Taxonomies
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Taxonomy), request.Id);
        }

        try
        {
            entity.Name = request.Taxonomy.Name;
            entity.Vocabulary = request.Taxonomy.Vocabulary;
            entity.Parent = request.Taxonomy.Parent;

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating taxonomy. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }
}


