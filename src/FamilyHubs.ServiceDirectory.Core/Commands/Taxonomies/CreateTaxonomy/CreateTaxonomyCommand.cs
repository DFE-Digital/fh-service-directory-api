using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.CreateTaxonomy;

public class CreateTaxonomyCommand : IRequest<long>
{
    public CreateTaxonomyCommand(TaxonomyDto taxonomy)
    {
        Taxonomy = taxonomy;
    }

    public TaxonomyDto Taxonomy { get; }
}

public class CreateTaxonomyCommandHandler : IRequestHandler<CreateTaxonomyCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTaxonomyCommandHandler> _logger;

    public CreateTaxonomyCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateTaxonomyCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<long> Handle(CreateTaxonomyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Taxonomies
                .FirstOrDefaultAsync(x => x.Name == request.Taxonomy.Name, cancellationToken);

            if (entity is not null)
                throw new ArgumentException($"Duplicate Taxonomy with Id:{request.Taxonomy.Id} Name:{request.Taxonomy.Name} Already Exists, Please use Update Command");

            var taxonomy = _mapper.Map<Taxonomy>(request.Taxonomy);

            _context.Taxonomies.Add(taxonomy);

            await _context.SaveChangesAsync(cancellationToken);

            return taxonomy.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating taxonomy. {exceptionMessage}", ex.Message);
            throw;
        }
    }
}