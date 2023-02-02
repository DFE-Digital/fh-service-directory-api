﻿using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;

public class CreateOrganisationCommand : IRequest<string>
{
    public CreateOrganisationCommand(OrganisationWithServicesDto organisation)
    {
        Organisation = organisation;
    }

    public OrganisationWithServicesDto Organisation { get; }
}

public class CreateOrganisationCommandHandler : IRequestHandler<CreateOrganisationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOrganisationCommandHandler> _logger;

    public CreateOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateOrganisationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Organisation>(request.Organisation);
            ArgumentNullException.ThrowIfNull(entity);

            if (_context.Organisations.FirstOrDefault(x => x.Id == request.Organisation.Id) != null)
            {
                throw new ArgumentException("Duplicate Id");
            }

            var organisationType = _context.OrganisationTypes.FirstOrDefault(x => x.Id == request.Organisation.OrganisationType.Id);
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

                    for(var i = service.Eligibilities.Count - 1; i >= 0; i--)
                    {
                        var eligibility = _context.Eligibilities.FirstOrDefault(x => x.Id == service.Eligibilities.ElementAt(i).Id);
                        if (eligibility != null)
                        {
                            service.Eligibilities.Remove(service.Eligibilities.ElementAt(i));

                            service.Eligibilities.Add(eligibility);
                        }
                    }

                    for (var i = service.Languages.Count - 1; i >= 0; i--)
                    {
                        var language = _context.Languages.FirstOrDefault(x => x.Id == service.Languages.ElementAt(i).Id);
                        if (language != null)
                        {
                            service.Languages.Remove(service.Languages.ElementAt(i));
                            service.Languages.Add(language);
                        }
                    }

                    foreach (var serviceTaxonomy in service.ServiceTaxonomies)
                    {
                        if (serviceTaxonomy.Taxonomy != null)
                        {
                            var taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == serviceTaxonomy.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                serviceTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }

                    foreach (var linkContact in service.LinkContacts)
                    {
                        if (linkContact.Contact != null)
                        {
                            var contact = _context.Contacts.FirstOrDefault(x => x.Id == linkContact.Contact.Id);
                            if (contact != null)
                            {
                                linkContact.Contact = contact;
                            }
                        }
                    }

                    foreach (var serviceAtLocation in service.ServiceAtLocations)
                    {
                        if (serviceAtLocation.RegularSchedules?.Any() == true)
                        {
                            foreach (var regularSchedules in serviceAtLocation.RegularSchedules)
                            {
                                regularSchedules.ServiceAtLocationId = serviceAtLocation.Id;
                            }
                        }

                        if (serviceAtLocation.HolidaySchedules?.Any() == true)
                        {
                            foreach (var holidaySchedules in serviceAtLocation.HolidaySchedules)
                            {
                                holidaySchedules.ServiceAtLocationId = serviceAtLocation.Id;
                            }
                        }

                        if (serviceAtLocation.LinkContacts?.Any() == true)
                        {
                            foreach (var linkContact in serviceAtLocation.LinkContacts)
                            {
                                if (linkContact.Contact != null)
                                {
                                    var contact = _context.Contacts.FirstOrDefault(x => x.Id == linkContact.Contact.Id);
                                    if (contact != null)
                                    {
                                        linkContact.Contact = contact;
                                    }
                                }
                            }
                        }

                        var existingLocation = await _context.Locations
                            .Include(l => l.PhysicalAddresses)
                            .Include(l => l.LinkTaxonomies)!
                            .ThenInclude(l => l.Taxonomy)
                            .Where(l => (l.Name == serviceAtLocation.Location.Name && serviceAtLocation.Location.Name != "") || l.Id == serviceAtLocation.Location.Id)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (existingLocation != null)
                        {
                            if (serviceAtLocation.Location.PhysicalAddresses != null)
                            {
                                foreach (var newAddresses in serviceAtLocation.Location.PhysicalAddresses)
                                {
                                    existingLocation.PhysicalAddresses ??= new List<PhysicalAddress>();
                                    if (existingLocation.PhysicalAddresses.All(a => a.PostCode != newAddresses.PostCode))
                                    {
                                        existingLocation.PhysicalAddresses.Add(newAddresses);
                                    }
                                }
                            }
                            serviceAtLocation.Location = existingLocation;
                        } 
                        
                        if (serviceAtLocation.Location.LinkTaxonomies?.Any() == true)
                        {
                            foreach (var linkTaxonomy in serviceAtLocation.Location.LinkTaxonomies)
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
                        
                        if (serviceAtLocation.Location.LinkContacts?.Any() == true)
                        {
                            foreach (var linkContact in serviceAtLocation.Location.LinkContacts)
                            {
                                if (linkContact.Contact != null)
                                {
                                    var contact = _context.Contacts.FirstOrDefault(x => x.Id == linkContact.Contact.Id);
                                    if (contact != null)
                                    {
                                        linkContact.Contact = contact;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            AddAdministrativeDistrict(request, entity);

            AddRelatedOrganisation(request, entity);

            entity.RegisterDomainEvent(new OrganisationCreatedEvent(entity));

            _context.Organisations.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw;
        }

        return request.Organisation.Id;
    }

    private void AddAdministrativeDistrict(CreateOrganisationCommand request, Organisation organisation)
    {
        if (!string.IsNullOrEmpty(request.Organisation.AdminAreaCode))
        {
            var organisationAdminDistrict = _context.AdminAreas.FirstOrDefault(x => x.OrganisationId == organisation.Id);
            if (organisationAdminDistrict == null)
            {
                var entity = new AdminArea(
                    Guid.NewGuid().ToString(),
                    request.Organisation.AdminAreaCode,
                    organisation.Id);

                _context.AdminAreas.Add(entity);
            }
        }
    }

    private void AddRelatedOrganisation(CreateOrganisationCommand request, Organisation organisation)
    {
        if (string.IsNullOrEmpty(request.Organisation.AdminAreaCode) || string.Compare(request.Organisation.OrganisationType.Name, "LA", StringComparison.OrdinalIgnoreCase) == 0)
            return;

        var result = (from admindis in _context.AdminAreas
                      join org in _context.Organisations
                           on admindis.OrganisationId equals org.Id
                      where admindis.Code == request.Organisation.AdminAreaCode
                      && org.OrganisationType.Name == "LA"
                      select org).FirstOrDefault();

        if (result == null)
        {
            _logger.LogError($"Unable to find Local Authority for: {request.Organisation.AdminAreaCode}");
            return;
        }

        var entity = new RelatedOrganisation(Guid.NewGuid().ToString(), result.Id, organisation.Id);
        entity.RegisterDomainEvent(new RelatedOrganisationCreatedEvent(entity));
        _context.RelatedOrganisations.Add(entity);
    }
}
