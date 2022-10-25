using Ardalis.Specification;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.core.Interfaces.Commands;
using fh_service_directory_api.core.Interfaces.Entities;
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

            var organisationType = _context.OrganisationTypes.FirstOrDefault(x => x.Id == request.OpenReferralOrganisation.OrganisationType.Id);
            if (organisationType != null)
            {
                entity.OrganisationType = organisationType;
            }

            if (entity.Services != null)
            {
                foreach (var service in entity.Services)
                {
                    var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == service.ServiceType.Id);
                    if (serviceType != null)
                        service.ServiceType = serviceType;

                    if (service.Service_taxonomys != null)
                    {
                        foreach (var servicetaxonomy in service.Service_taxonomys)
                        {
                            if (servicetaxonomy != null && servicetaxonomy.Taxonomy != null)
                            {
                                var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == servicetaxonomy.Taxonomy.Id);
                                if (taxonomy != null)
                                {
                                    servicetaxonomy.Taxonomy = taxonomy;
                                }
                            }
                        }
                    }
                }
            }

            AddAdministractiveDistrict(request, entity);

            AddRelatedOrganisation(request, entity);

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

    private void AddAdministractiveDistrict(CreateOpenReferralOrganisationCommand request, OpenReferralOrganisation openReferralOrganisation)
    {
        if (!string.IsNullOrEmpty(request.OpenReferralOrganisation.AdministractiveDistrictCode))
        {
            var organisationAdminDistrict = _context.OrganisationAdminDistricts.FirstOrDefault(x => x.OpenReferralOrganisationId == openReferralOrganisation.Id);
            if (organisationAdminDistrict == null)
            {
                var entity = new OrganisationAdminDistrict(
                    Guid.NewGuid().ToString(),
                    request.OpenReferralOrganisation.AdministractiveDistrictCode,
                    openReferralOrganisation.Id);

                entity.RegisterDomainEvent(new OrganisationAdminDistrictCreatedEvent(entity));
                _context.OrganisationAdminDistricts.Add(entity);
            }
        }
    }

    private void AddRelatedOrganisation(CreateOpenReferralOrganisationCommand request, OpenReferralOrganisation openReferralOrganisation)
    {
        if (string.IsNullOrEmpty(request.OpenReferralOrganisation.AdministractiveDistrictCode) || string.Compare(request.OpenReferralOrganisation.OrganisationType.Name,"LA", StringComparison.OrdinalIgnoreCase) == 0)
            return;

        var result = (from admindis in _context.OrganisationAdminDistricts
                  join org in _context.OpenReferralOrganisations
                       on admindis.OpenReferralOrganisationId equals org.Id
                       where admindis.Code == request.OpenReferralOrganisation.AdministractiveDistrictCode
                       && org.OrganisationType.Name == "LA"
                       select org).FirstOrDefault();

        if (result == null)
        {
            _logger.LogError($"Unable to find Local Authority for: {request.OpenReferralOrganisation.AdministractiveDistrictCode}");
            return;
        }

        var entity = new RelatedOrganisation(result.Id, openReferralOrganisation.Id);
        entity.RegisterDomainEvent(new RelatedOrganisationCreatedEvent(entity));
        _context.RelatedOrganisations.Add(entity);
    }
}
