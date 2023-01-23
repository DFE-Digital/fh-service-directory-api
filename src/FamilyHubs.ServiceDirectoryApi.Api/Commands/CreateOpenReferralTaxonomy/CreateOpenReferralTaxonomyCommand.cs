using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Commands.CreateOpenReferralTaxonomy;

public class CreateOpenReferralTaxonomyCommand : IRequest<string>
{
    public CreateOpenReferralTaxonomyCommand(OpenReferralTaxonomyDto openReferralTaxonomy)
    {
        OpenReferralTaxonomy = openReferralTaxonomy;
    }

    public OpenReferralTaxonomyDto OpenReferralTaxonomy { get; init; }
}

public class CreateOpenReferralTaxonomyCommandHandler : IRequestHandler<CreateOpenReferralTaxonomyCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOpenReferralTaxonomyCommandHandler> _logger;

    public CreateOpenReferralTaxonomyCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateOpenReferralTaxonomyCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateOpenReferralTaxonomyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<OpenReferralTaxonomy>(request.OpenReferralTaxonomy);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            entity.RegisterDomainEvent(new OpenReferralTaxonomyCreatedEvent(entity));

            _context.OpenReferralTaxonomies.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating taxonomy. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        if (request is not null && request.OpenReferralTaxonomy is not null)
            return request.OpenReferralTaxonomy.Id;
        return string.Empty;
    }
}