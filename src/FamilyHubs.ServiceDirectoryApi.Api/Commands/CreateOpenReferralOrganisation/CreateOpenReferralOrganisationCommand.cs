using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.core.Interfaces.Commands;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;

public class CreateOpenReferralOrganisationCommand : IRequest<string>, ICreateOpenReferralOrganisationCommand
{
    public CreateOpenReferralOrganisationCommand(OpenReferralOrganisationWithServicesDto openReferralOrganisation)
    {
        OpenReferralOrganisation = openReferralOrganisation;
    }

    public OpenReferralOrganisationWithServicesDto OpenReferralOrganisation { get; init; }
}

public class CreateOpenReferralOrganisationCommandHandler : IRequestHandler<CreateOpenReferralOrganisationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOpenReferralOrganisationCommandHandler> _logger;

    public CreateOpenReferralOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateOpenReferralOrganisationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<OpenReferralOrganisation>(request.OpenReferralOrganisation);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            entity.RegisterDomainEvent(new OpenReferralOrganisationCreatedEvent(entity));

            _context.OpenReferralOrganisations.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        if (request is not null && request.OpenReferralOrganisation is not null)
            return request.OpenReferralOrganisation.Id;
        else
            return string.Empty;
    }
}
