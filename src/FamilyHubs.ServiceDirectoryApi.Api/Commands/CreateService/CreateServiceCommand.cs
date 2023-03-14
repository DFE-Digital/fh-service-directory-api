using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateService;

public class CreateServiceCommand : IRequest<string>
{
    public CreateServiceCommand(ServiceDto service)
    {
        Service = service;
    }

    public ServiceDto Service { get; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateServiceCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var unsavedEntity = _mapper.Map<Service>(request.Service);
            ArgumentNullException.ThrowIfNull(unsavedEntity);

            var existing = _context.Services.FirstOrDefault(e => unsavedEntity.Id == e.Id);

            if (existing is not null)
                throw new InvalidOperationException($"Service with Id: {unsavedEntity.Id} already exists, Please use Update command");

            if (unsavedEntity != null && unsavedEntity.ServiceAtLocations != null)
            {
                await CreateRegularSchedulesThatDoNotExist(unsavedEntity, cancellationToken); 
            }
           
            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.Service.ServiceType.Id);
            if (serviceType != null)
                unsavedEntity.ServiceType = serviceType;

            unsavedEntity.Eligibilities = AttachExistingEligibility(unsavedEntity.Eligibilities);
            unsavedEntity.ServiceAreas = AttachExistingServiceArea(unsavedEntity.ServiceAreas);
            unsavedEntity.ServiceDeliveries = AttachExistingServiceDelivery(unsavedEntity.ServiceDeliveries);
            unsavedEntity.LinkContacts = AttachExistingContacts(unsavedEntity.LinkContacts, null);
            unsavedEntity.Languages = AttachExistingLanguages(unsavedEntity.Languages);
            unsavedEntity.ServiceTaxonomies = AttachExistingServiceTaxonomies(unsavedEntity.ServiceTaxonomies);
            unsavedEntity.CostOptions = AttachExistingCostOptions(unsavedEntity.CostOptions);
            unsavedEntity.RegularSchedules = AttachExistingRegularSchedule(unsavedEntity.RegularSchedules, null);
            unsavedEntity.HolidaySchedules = AttachExistingHolidaySchedule(unsavedEntity.HolidaySchedules, null);

            unsavedEntity.ServiceAtLocations = AttachExistingServiceAtLocation(unsavedEntity.ServiceAtLocations);

            _context.Services.Add(unsavedEntity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating service. {exceptionMessage}", ex.Message);
            throw;
        }

        return request.Service.Id;
    }

    private async Task CreateRegularSchedulesThatDoNotExist(Service unsavedEntity, CancellationToken cancellationToken)
    {
        bool added = false;
        if (unsavedEntity != null && unsavedEntity.ServiceAtLocations != null)
        {
            var regularSchedules = unsavedEntity.ServiceAtLocations.Select(x => x.RegularSchedules);
            if (regularSchedules != null && regularSchedules.Any()) 
            {
                foreach (ICollection<RegularSchedule>? regularSchedulecollection in regularSchedules)
                {
                    if (regularSchedulecollection != null && regularSchedulecollection.Any())
                    {
                        foreach (RegularSchedule regularSchedule in regularSchedulecollection)
                        {
                            if (regularSchedule != null)
                            {
                                var schedule = _context.RegularSchedules.SingleOrDefault(x => x.Id == regularSchedule.Id);
                                if (schedule == null)
                                {
                                    _context.RegularSchedules.Add(regularSchedule);
                                    added = true;
                                }
                            }
                        }

                        if (added)
                        {
                            await _context.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
            }
        }
    }

    private ICollection<Eligibility> AttachExistingEligibility(ICollection<Eligibility>? unSavedEntities)
    {
        var returnList = new List<Eligibility>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Eligibilities.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<ServiceDelivery> AttachExistingServiceDelivery(ICollection<ServiceDelivery>? unSavedEntities)
    {
        var returnList = new List<ServiceDelivery>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ServiceDeliveries.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<Language> AttachExistingLanguages(ICollection<Language>? unSavedEntities)
    {
        var returnList = new List<Language>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Languages.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<CostOption> AttachExistingCostOptions(ICollection<CostOption>? unSavedEntities)
    {
        var returnList = new List<CostOption>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.CostOptions.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<ServiceArea> AttachExistingServiceArea(ICollection<ServiceArea>? unSavedEntities)
    {
        var returnList = new List<ServiceArea>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ServiceAreas.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<ServiceTaxonomy> AttachExistingServiceTaxonomies(ICollection<ServiceTaxonomy>? unSavedEntities)
    {
        var returnList = new List<ServiceTaxonomy>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ServiceTaxonomies
            .Include(t => t.Taxonomy)
            .Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        var newIds = unSavedEntities.Where(c => c.Taxonomy != null).Select(c => c.Taxonomy!.Id).ToList();
        var existingChilds = _context.Taxonomies.Where(x => newIds.Contains(x.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            var itemToSave = savedItem ?? unSavedItem;

            if (itemToSave.Taxonomy != null)
            {
                var existingTaxonomy = existing.Select(t => t.Taxonomy).SingleOrDefault(t => t!.Id == itemToSave.Taxonomy.Id)
                                       ?? existingChilds.SingleOrDefault(t => t.Id == itemToSave.Taxonomy.Id);
                if (existingTaxonomy is not null)
                {
                    itemToSave.Taxonomy = existingTaxonomy;
                }
            }

            returnList.Add(itemToSave);
        }

        return returnList;
    }

    private ICollection<ServiceAtLocation> AttachExistingServiceAtLocation(ICollection<ServiceAtLocation>? unSavedEntities)
    {
        var returnList = new List<ServiceAtLocation>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ServiceAtLocations
            .Include(l => l.HolidaySchedules)
            .Include(l => l.RegularSchedules)
            .Include(l => l.LinkContacts!)
            .ThenInclude(l => l.Contact)
            .Include(l => l.Location)
                .ThenInclude(l => l.PhysicalAddresses)
            .Include(l => l.Location)
                .ThenInclude(l => l.LinkTaxonomies)!
                .ThenInclude(l => l.Taxonomy)
            .Include(l => l.Location)
                .ThenInclude(l => l.LinkContacts)!
                .ThenInclude(l => l.Contact)
            .Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            returnList.Add(AttachExistingServiceAtLocationChildEntities(unSavedItem, savedItem));
        }

        return returnList;
    }

    private ServiceAtLocation AttachExistingServiceAtLocationChildEntities(ServiceAtLocation unSavedEntity, ServiceAtLocation? existing)
    {
        //Update Service at Location Level data
        var returnItem = existing ?? unSavedEntity;

        returnItem.RegularSchedules = AttachExistingRegularSchedule(unSavedEntity.RegularSchedules, existing?.RegularSchedules);
        returnItem.HolidaySchedules = AttachExistingHolidaySchedule(unSavedEntity.HolidaySchedules, existing?.HolidaySchedules);
        returnItem.LinkContacts = AttachExistingContacts(unSavedEntity.LinkContacts, existing?.LinkContacts);
        returnItem.Location = AttachExistingLocation(unSavedEntity.Location, existing?.Location);

        return returnItem;
    }

    private Location AttachExistingLocation(Location unSavedEntity, Location? existing)
    {
        existing ??= _context.Locations.SingleOrDefault(l => !string.IsNullOrWhiteSpace(l.Name) && !string.IsNullOrWhiteSpace(unSavedEntity.Name) && l.Name == unSavedEntity.Name);

        var returnItem = existing ?? unSavedEntity;

        returnItem.PhysicalAddresses = AttachExistingPhysicalAddress(unSavedEntity.PhysicalAddresses, existing?.PhysicalAddresses);

        returnItem.LinkTaxonomies = AttachExistingLinkTaxonomy(unSavedEntity.LinkTaxonomies, existing?.LinkTaxonomies);

        returnItem.LinkContacts = AttachExistingContacts(unSavedEntity.LinkContacts, existing?.LinkContacts);

        return returnItem;
    }

    private ICollection<HolidaySchedule> AttachExistingHolidaySchedule(ICollection<HolidaySchedule>? unSavedEntities, ICollection<HolidaySchedule>? existing)
    {
        var returnList = new List<HolidaySchedule>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        existing ??= _context.HolidaySchedules.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<RegularSchedule> AttachExistingRegularSchedule(ICollection<RegularSchedule>? unSavedEntities, ICollection<RegularSchedule>? existing)
    {
        var returnList = new List<RegularSchedule>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        existing ??= _context.RegularSchedules.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<PhysicalAddress> AttachExistingPhysicalAddress(ICollection<PhysicalAddress>? unSavedEntities, ICollection<PhysicalAddress>? existing)
    {
        var returnList = new List<PhysicalAddress>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        existing ??= _context.PhysicalAddresses.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<LinkTaxonomy> AttachExistingLinkTaxonomy(ICollection<LinkTaxonomy>? unSavedEntities, ICollection<LinkTaxonomy>? existing)
    {
        var returnList = new List<LinkTaxonomy>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        existing ??= _context.LinkTaxonomies
            .Include(t => t.Taxonomy)
            .Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        var newIds = unSavedEntities.Where(c => c.Taxonomy != null).Select(c => c.Taxonomy!.Id).ToList();
        var existingChilds = _context.Taxonomies.Where(x => newIds.Contains(x.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            var itemToSave = savedItem ?? unSavedItem;

            if (itemToSave.Taxonomy != null)
            {
                var existingTaxonomy = existing.Select(t => t.Taxonomy).SingleOrDefault(t => t!.Id == itemToSave.Taxonomy.Id)
                                       ?? existingChilds.SingleOrDefault(t => t.Id == itemToSave.Taxonomy.Id);

                if (existingTaxonomy is not null)
                {
                    itemToSave.Taxonomy = existingTaxonomy;
                }
            }

            returnList.Add(itemToSave);
        }

        return returnList;
    }

    private ICollection<LinkContact> AttachExistingContacts(ICollection<LinkContact>? unSavedEntities, ICollection<LinkContact>? existing)
    {
        var returnList = new List<LinkContact>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        existing ??= _context.LinkContacts
            .Include(t => t.Contact)
            .Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        var newIds = unSavedEntities.Where(c => c.Contact != null).Select(c => c.Contact!.Id).ToList();
        var existingChilds = _context.Contacts.Where(x => newIds.Contains(x.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            var itemToSave = savedItem ?? unSavedItem;

            if (itemToSave.Contact != null)
            {
                var existingContact = existing.Select(t => t.Contact).SingleOrDefault(t => t!.Id == itemToSave.Contact.Id)
                                      ?? existingChilds.SingleOrDefault(t => t.Id == itemToSave.Contact.Id);

                if (existingContact is not null)
                {
                    itemToSave.Contact = existingContact;
                }
            }

            returnList.Add(itemToSave);
        }

        return returnList;
    }
}