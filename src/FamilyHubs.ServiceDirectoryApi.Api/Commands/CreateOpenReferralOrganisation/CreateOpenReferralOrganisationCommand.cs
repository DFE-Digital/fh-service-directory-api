using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.core.Interfaces.Commands;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            if (_context.OpenReferralOrganisations.FirstOrDefault(x => x.Id == request.OpenReferralOrganisation.Id) != null)
            {
                throw new ArgumentException("Duplicate Id");
            }

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

                    foreach (var serviceTaxonomy in service.Service_taxonomys)
                    {
                        if (serviceTaxonomy.Taxonomy != null)
                        {
                            var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == serviceTaxonomy.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                serviceTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }

                    foreach (var serviceAtLocation in service.Service_at_locations)
                    {
                        if (serviceAtLocation.Regular_schedule != null)
                        {
                            foreach (var regularSchedules in serviceAtLocation.Regular_schedule)
                            {
                                regularSchedules.OpenReferralServiceAtLocationId = serviceAtLocation.Id;
                            }
                        }

                        if (serviceAtLocation.HolidayScheduleCollection != null)
                        {
                            foreach (var holidaySchedules in serviceAtLocation.HolidayScheduleCollection)
                            {
                                holidaySchedules.OpenReferralServiceAtLocationId = serviceAtLocation.Id;
                            }
                        }

                        var existingLocation = await _context.OpenReferralLocations
                            .Include(l => l.Physical_addresses)
                            .Include(l => l.LinkTaxonomies)!
                            .ThenInclude(l => l.Taxonomy)
                            .Where(l => l.Name == serviceAtLocation.Location.Name)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (existingLocation != null)
                        {
                            if (serviceAtLocation.Location.Physical_addresses != null)
                            {
                                foreach (var newAddresses in serviceAtLocation.Location.Physical_addresses)
                                {
                                    existingLocation.Physical_addresses ??= new List<OpenReferralPhysical_Address>();
                                    if (existingLocation.Physical_addresses.All(a => a.Postal_code != newAddresses.Postal_code))
                                    {
                                        existingLocation.Physical_addresses.Add(newAddresses);
                                    }
                                }
                            }
                            serviceAtLocation.Location = existingLocation;
                        }
                        else if (serviceAtLocation.Location.LinkTaxonomies != null)
                        {
                            foreach (var linkTaxonomy in serviceAtLocation.Location.LinkTaxonomies)
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
                    }
                }
            }

            AddAdministrativeDistrict(request, entity);

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

        return request.OpenReferralOrganisation.Id;
    }

    private void AddAdministrativeDistrict(CreateOpenReferralOrganisationCommand request, OpenReferralOrganisation openReferralOrganisation)
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
        if (string.IsNullOrEmpty(request.OpenReferralOrganisation.AdministractiveDistrictCode) || string.Compare(request.OpenReferralOrganisation.OrganisationType.Name, "LA", StringComparison.OrdinalIgnoreCase) == 0)
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

        var entity = new RelatedOrganisation(Guid.NewGuid().ToString(), result.Id, openReferralOrganisation.Id);
        entity.RegisterDomainEvent(new RelatedOrganisationCreatedEvent(entity));
        _context.RelatedOrganisations.Add(entity);
    }
}
