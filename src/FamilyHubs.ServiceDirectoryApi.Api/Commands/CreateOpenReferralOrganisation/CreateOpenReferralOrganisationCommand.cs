using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;

public class CreateOpenReferralOrganisationCommand : IRequest<string>
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

    public CreateOpenReferralOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
            throw new Exception(ex.Message, ex);
        }

        if (request is not null && request.OpenReferralOrganisation is not null)
            return request.OpenReferralOrganisation.Id;
        else
            return string.Empty;
    }
}
