using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Commands.CreateLocation;

public class CreateOpenReferralLocationCommand : IRequest<string>
{
    public CreateOpenReferralLocationCommand(OpenReferralLocationDto openReferralLocationDto)
    {
        OpenReferralLocationDto = openReferralLocationDto;
    }

    public OpenReferralLocationDto OpenReferralLocationDto { get; init; }
}

public class CreateOpenReferralLocationCommandHandler : IRequestHandler<CreateOpenReferralLocationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOpenReferralLocationCommandHandler> _logger;
    public CreateOpenReferralLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateOpenReferralLocationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateOpenReferralLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingLocation = await _context.OpenReferralLocations
                .Where(l => l.Name == request.OpenReferralLocationDto.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingLocation != null)
            {
                throw new InvalidOperationException("Location Already Exists, Please use Update Location");
            }

            var entity = _mapper.Map<OpenReferralLocation>(request.OpenReferralLocationDto);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            if (entity.LinkTaxonomies != null)
            {
                foreach (var linkTaxonomy in entity.LinkTaxonomies)
                {
                    if (linkTaxonomy.Taxonomy != null)
                    {
                        var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == linkTaxonomy.Taxonomy.Id);
                        if (taxonomy != null)
                        {
                            linkTaxonomy.Taxonomy = taxonomy;
                        }
                    }
                }
            }

            _context.OpenReferralLocations.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            entity.RegisterDomainEvent(new OpenReferralLocationCreatedEvent(entity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating Location. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return request.OpenReferralLocationDto.Id;
    }
}
