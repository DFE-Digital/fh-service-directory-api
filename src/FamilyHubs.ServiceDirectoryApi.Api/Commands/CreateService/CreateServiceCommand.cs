using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateService;

public class CreateServiceCommand : IRequest<long>
{
    public CreateServiceCommand(ServiceDto service)
    {
        Service = service;
    }

    public ServiceDto Service { get; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateServiceCommandHandler> _logger;
    private List<Contact> _dbContacts = new List<Contact>();

    public CreateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateServiceCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<long> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var unsavedEntity = _mapper.Map<Service>(request.Service);
            ArgumentNullException.ThrowIfNull(unsavedEntity);

            var existing = _context.Services.FirstOrDefault(e => unsavedEntity.Id == e.Id);

            if (existing is not null)
                throw new InvalidOperationException($"Service with Id: {unsavedEntity.Id} already exists, Please use Update command");

            _dbContacts = await _context.Contacts.ToListAsync(cancellationToken);

            unsavedEntity.Eligibilities = AttachExistingEligibility(unsavedEntity.Eligibilities);
            unsavedEntity.ServiceAreas = AttachExistingServiceArea(unsavedEntity.ServiceAreas);
            unsavedEntity.ServiceDeliveries = AttachExistingServiceDelivery(unsavedEntity.ServiceDeliveries);
            unsavedEntity.Contacts = AttachExistingContacts(unsavedEntity.Contacts, _dbContacts);
            unsavedEntity.Languages = AttachExistingLanguages(unsavedEntity.Languages);
            unsavedEntity.Taxonomies = AttachExistingServiceTaxonomies(unsavedEntity.Taxonomies);
            unsavedEntity.CostOptions = AttachExistingCostOptions(unsavedEntity.CostOptions);
            unsavedEntity.RegularSchedules = AttachExistingRegularSchedule(unsavedEntity.RegularSchedules, null);
            unsavedEntity.HolidaySchedules = AttachExistingHolidaySchedule(unsavedEntity.HolidaySchedules, null);

            unsavedEntity.Locations = AttachExistingLocation(unsavedEntity.Locations);

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

    private ICollection<Taxonomy> AttachExistingServiceTaxonomies(ICollection<Taxonomy>? unSavedEntities)
    {
        var returnList = new List<Taxonomy>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Taxonomies.Where(e => unSavedEntities.Select(c => c.Name).Contains(e.Name)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<Location> AttachExistingLocation(ICollection<Location>? unSavedEntities)
    {
        var returnList = new List<Location>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = (from dbLocation in _context.Locations
                join newLocation in unSavedEntities on
                    new {dbLocation.Name, dbLocation.PostCode} equals 
                    new {newLocation.Name, newLocation.PostCode}
                select dbLocation)
            .ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            var returnItem = savedItem ?? unSavedItem;
            
            //TODO implement AccessibilityForDisabilities when we start to have this data
            //returnItem.AccessibilityForDisabilities = AttachExistingAccessibilityForDisabilities()
            returnItem.Contacts = AttachExistingContacts(unSavedItem.Contacts, _dbContacts);
            returnItem.HolidaySchedules = AttachExistingHolidaySchedule(unSavedItem.HolidaySchedules, savedItem?.HolidaySchedules);
            returnItem.RegularSchedules = AttachExistingRegularSchedule(unSavedItem.RegularSchedules, savedItem?.RegularSchedules);
            
            returnList.Add(returnItem);
        }

        return returnList;
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
    
    private ICollection<Contact> AttachExistingContacts(ICollection<Contact>? unSavedEntities, ICollection<Contact> allContacts)
    {
        var returnList = new List<Contact>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = (from dbContact in allContacts
            join newContact in unSavedEntities on
                new { dbContact.Name, dbContact.Email, dbContact.Telephone } equals 
                new { newContact.Name, newContact.Email, newContact.Telephone }
            select dbContact)
            .ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }
}