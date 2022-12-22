using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.ModelLink;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using fh_service_directory_api.api.Commands.CreateModelLink;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Commands.CreateLocation;

public class CreateOpenReferralLocationCommand : IRequest<string>
{
    public CreateOpenReferralLocationCommand(OpenReferralLocationDto openReferralLocationDto, string openReferralTaxonomyId, string openReferralOrganisationId)
    {
        OpenReferralLocationDto = openReferralLocationDto;
        OpenReferralTaxonomyId = openReferralTaxonomyId;
        OpenReferralOrganisationId = openReferralOrganisationId;
    }

    public OpenReferralLocationDto OpenReferralLocationDto { get; init; }
    public string OpenReferralTaxonomyId { get; init; } = default!;
    public string OpenReferralOrganisationId { get; init; } = default!;
}

public class CreateOpenReferralLocationCommandHandler : IRequestHandler<CreateOpenReferralLocationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISender _mediator;
    private readonly ILogger<CreateOpenReferralLocationCommandHandler> _logger;
    public CreateOpenReferralLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ISender mediator, ILogger<CreateOpenReferralLocationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;   
        _logger = logger;
    }

    public async Task<string> Handle(CreateOpenReferralLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<OpenReferralLocation>(request.OpenReferralLocationDto);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            entity.RegisterDomainEvent(new OpenReferralLocationCreatedEvent(entity));

            _context.OpenReferralLocations.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            ModelLinkDto modelLinkDto = new ModelLinkDto(Guid.NewGuid().ToString(), fh_service_directory_api.core.LinkType.Location, entity.Id, request.OpenReferralTaxonomyId);
            CreateModelLinkCommand command = new CreateModelLinkCommand(modelLinkDto);
            var taxonomyLinkresult = await _mediator.Send(command, cancellationToken);
            ArgumentNullException.ThrowIfNull(taxonomyLinkresult, nameof(taxonomyLinkresult));

            modelLinkDto = new ModelLinkDto(Guid.NewGuid().ToString(), fh_service_directory_api.core.LinkType.Location_Organisation, entity.Id, request.OpenReferralOrganisationId);
            command = new CreateModelLinkCommand(modelLinkDto);
            var organisationLinkresult = await _mediator.Send(command, cancellationToken);
            ArgumentNullException.ThrowIfNull(organisationLinkresult, nameof(organisationLinkresult));


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating Location. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        if (request is not null && request.OpenReferralLocationDto is not null)
            return request.OpenReferralLocationDto.Id;
        else
            return string.Empty;
    }
}
