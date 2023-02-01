using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateTaxonomy;

public class CreateTaxonomyCommand : IRequest<string>
{
    public CreateTaxonomyCommand(TaxonomyDto taxonomy)
    {
        Taxonomy = taxonomy;
    }

    public TaxonomyDto Taxonomy { get; init; }
}

public class CreateTaxonomyCommandHandler : IRequestHandler<CreateTaxonomyCommand, string>
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

    public async Task<string> Handle(CreateTaxonomyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Taxonomy>(request.Taxonomy);
            ArgumentNullException.ThrowIfNull(entity);

            entity.RegisterDomainEvent(new TaxonomyCreatedEvent(entity));

            _context.Taxonomies.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating taxonomy. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return request.Taxonomy.Id;
    }
}