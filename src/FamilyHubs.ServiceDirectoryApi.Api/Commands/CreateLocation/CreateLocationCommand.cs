using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateLocation;

public class CreateLocationCommand : IRequest<string>
{
    public CreateLocationCommand(LocationDto locationDto)
    {
        LocationDto = locationDto;
    }

    public LocationDto LocationDto { get; }
}

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateLocationCommandHandler> _logger;
    public CreateLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateLocationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingLocation = await _context.Locations
                .Where(l => l.Name == request.LocationDto.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingLocation != null)
            {
                throw new InvalidOperationException("Location Already Exists, Please use Update Location");
            }

            var entity = _mapper.Map<Location>(request.LocationDto);

            ArgumentNullException.ThrowIfNull(entity);

            if (entity.LinkTaxonomies != null)
            {
                foreach (var linkTaxonomy in entity.LinkTaxonomies)
                {
                    if (linkTaxonomy.Taxonomy != null)
                    {
                        var taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == linkTaxonomy.Taxonomy.Id);
                        if (taxonomy != null)
                        {
                            linkTaxonomy.Taxonomy = taxonomy;
                        }
                    }
                }
            }

            _context.Locations.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating Location. {exceptionMessage}", ex.Message);
            throw;
        }

        return request.LocationDto.Id;
    }
}
