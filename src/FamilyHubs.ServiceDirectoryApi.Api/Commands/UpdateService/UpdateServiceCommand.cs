using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;

public class UpdateServiceCommand : IRequest<string>
{
    public UpdateServiceCommand(string id, ServiceDto service)
    {
        Id = id;
        Service = service;
    }

    public ServiceDto Service { get; }

    public string Id { get; }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateServiceCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.Services.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Service), request.Id);
        }

        try
        {
            var serviceEntity = _mapper.Map<Service>(request.Service);
            ArgumentNullException.ThrowIfNull(serviceEntity);

            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.Service.ServiceType.Id);
            if (serviceType != null)
                entity.ServiceType = serviceType;

            entity.Name = serviceEntity.Name;
            entity.Description = serviceEntity.Description;
            entity.Accreditations = serviceEntity.Accreditations;
            entity.AssuredDate = serviceEntity.AssuredDate;
            entity.AttendingAccess = serviceEntity.AttendingAccess;
            entity.AttendingType = serviceEntity.AttendingType;
            entity.DeliverableType = serviceEntity.DeliverableType;
            entity.Status = serviceEntity.Status;
            entity.Fees = serviceEntity.Fees;

            entity.Eligibilities = AttachExistingEligibility(request.Service.Eligibilities);
            entity.ServiceAreas = AttachExistingServiceArea(request.Service.ServiceAreas);
            entity.ServiceDeliveries = AttachExistingServiceDelivery(request.Service.ServiceDeliveries);
            entity.LinkContacts = AttachExistingContacts(request.Service.LinkContacts, entity.Id, null, null);
            entity.Languages = AttachExistingLanguages(request.Service.Languages);
            entity.ServiceAtLocations = AttachExistingServiceAtLocation(request.Service.ServiceAtLocations);
            entity.ServiceTaxonomies = AttachExistingServiceTaxonomies(request.Service.ServiceTaxonomies);
            entity.CostOptions = AttachExistingCostOptions(request.Service.CostOptions);
            entity.RegularSchedules = AttachExistingRegularSchedule(request.Service.RegularSchedules, entity.Id, null);
            entity.HolidaySchedules = AttachExistingHolidaySchedule(request.Service.HolidaySchedules, entity.Id, null);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw;
        }

        return entity.Id;
    }

    private ICollection<Eligibility> AttachExistingEligibility(ICollection<EligibilityDto>? unSavedEntities)
    {
        var returnList = new List<Eligibility>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Eligibilities.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.EligibilityDescription = unSavedItem.EligibilityDescription;
                savedItem.MaximumAge = unSavedItem.MaximumAge;
                savedItem.MinimumAge = unSavedItem.MinimumAge;
            }

            returnList.Add(savedItem ?? _mapper.Map<Eligibility>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<ServiceDelivery> AttachExistingServiceDelivery(ICollection<ServiceDeliveryDto>? unSavedEntities)
    {
        var returnList = new List<ServiceDelivery>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ServiceDeliveries.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.Name = unSavedItem.Name;
            }

            returnList.Add(savedItem ?? _mapper.Map<ServiceDelivery>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<Language> AttachExistingLanguages(ICollection<LanguageDto>? unSavedEntities)
    {
        var returnList = new List<Language>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Languages.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            
            if (savedItem is not null)
            {
                savedItem.Name = unSavedItem.Name;
            }

            returnList.Add(savedItem ?? _mapper.Map<Language>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<CostOption> AttachExistingCostOptions(ICollection<CostOptionDto>? unSavedEntities)
    {
        var returnList = new List<CostOption>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.CostOptions.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            
            if (savedItem is not null)
            {
                savedItem.Amount = unSavedItem.Amount;
                savedItem.AmountDescription = unSavedItem.AmountDescription;
                savedItem.Option = unSavedItem.Option;
                savedItem.ValidFrom = unSavedItem.ValidFrom;
                savedItem.ValidTo = unSavedItem.ValidTo;
            }

            returnList.Add(savedItem ?? _mapper.Map<CostOption>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<ServiceArea> AttachExistingServiceArea(ICollection<ServiceAreaDto>? unSavedEntities)
    {
        var returnList = new List<ServiceArea>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ServiceAreas.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.Extent = unSavedItem.Extent;
                savedItem.ServiceAreaDescription = unSavedItem.ServiceAreaDescription;
                savedItem.Uri = unSavedItem.Uri;
            }

            returnList.Add(savedItem ?? _mapper.Map<ServiceArea>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<ServiceTaxonomy> AttachExistingServiceTaxonomies(ICollection<ServiceTaxonomyDto>? unSavedEntities)
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

            var itemToSave = savedItem ?? _mapper.Map<ServiceTaxonomy>(unSavedItem);

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

    private ICollection<ServiceAtLocation> AttachExistingServiceAtLocation(ICollection<ServiceAtLocationDto>? unSavedEntities)
    {
        var returnList = new List<ServiceAtLocation>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ServiceAtLocations
            .Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            returnList.Add(AttachExistingServiceAtLocationChildEntities(unSavedItem, savedItem));
        }

        return returnList;
    }

    private ServiceAtLocation AttachExistingServiceAtLocationChildEntities(ServiceAtLocationDto unSavedEntity, ServiceAtLocation? savedItem)
    {
        //Update Service at Location Level data
        var returnItem = savedItem ?? _mapper.Map<ServiceAtLocation>(unSavedEntity);
        returnItem.RegularSchedules = AttachExistingRegularSchedule(unSavedEntity.RegularSchedules, null, unSavedEntity.Id);
        returnItem.HolidaySchedules = AttachExistingHolidaySchedule(unSavedEntity.HolidaySchedules, null, unSavedEntity.Id);
        returnItem.LinkContacts = AttachExistingContacts(unSavedEntity.LinkContacts, null, unSavedEntity.Id, null);
        returnItem.Location = AttachExistingLocation(unSavedEntity.Location);
        returnItem.LocationId = returnItem.Location.Id;

        return returnItem;
    }

    private Location AttachExistingLocation(LocationDto unSavedEntity)
    {
        var savedItem = _context.Locations.FirstOrDefault(e => e.Id == unSavedEntity.Id || (!string.IsNullOrWhiteSpace(e.Name) && !string.IsNullOrWhiteSpace(unSavedEntity.Name) && e.Name == unSavedEntity.Name));
        
        if (savedItem is not null)
        {
            savedItem.Name = unSavedEntity.Name;
            savedItem.Description = unSavedEntity.Description;
            savedItem.Latitude = unSavedEntity.Latitude;
            savedItem.Longitude = unSavedEntity.Longitude;
        }

        var returnItem = savedItem ?? _mapper.Map<Location>(unSavedEntity);

        returnItem.PhysicalAddresses = AttachExistingPhysicalAddress(unSavedEntity.PhysicalAddresses, unSavedEntity.Id);

        returnItem.LinkTaxonomies = AttachExistingLinkTaxonomy(unSavedEntity.LinkTaxonomies, null, unSavedEntity.Id, null);

        returnItem.LinkContacts = AttachExistingContacts(unSavedEntity.LinkContacts, null, null, unSavedEntity.Id);

        return returnItem;
    }

    private ICollection<HolidaySchedule> AttachExistingHolidaySchedule(ICollection<HolidayScheduleDto>? unSavedEntities, string? serviceId, string? serviceAtLocationId)
    {
        var returnList = new List<HolidaySchedule>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.HolidaySchedules.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = _mapper.Map<HolidaySchedule>(unSavedEntities.ElementAt(i));
            unSavedItem.ServiceId = serviceId!;
            unSavedItem.ServiceAtLocationId = serviceAtLocationId!;

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.Closed = unSavedItem.Closed;
                savedItem.OpensAt = unSavedItem.OpensAt;
                savedItem.ClosesAt = unSavedItem.ClosesAt;
                savedItem.EndDate = unSavedItem.EndDate;
                savedItem.StartDate = unSavedItem.StartDate;
            }

            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<RegularSchedule> AttachExistingRegularSchedule(ICollection<RegularScheduleDto>? unSavedEntities, string? serviceId, string? serviceAtLocationId)
    {
        var returnList = new List<RegularSchedule>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.RegularSchedules.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = _mapper.Map<RegularSchedule>(unSavedEntities.ElementAt(i));
            unSavedItem.ServiceId = serviceId!;
            unSavedItem.ServiceAtLocationId = serviceAtLocationId!;

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            
            if (savedItem is not null)
            {
                savedItem.ByDay = unSavedItem.ByDay;
                savedItem.ByMonthDay = unSavedItem.ByMonthDay;
                savedItem.Description = unSavedItem.Description;
                savedItem.OpensAt = unSavedItem.OpensAt;
                savedItem.ClosesAt = unSavedItem.ClosesAt;
                savedItem.DtStart = unSavedItem.DtStart;
                savedItem.Freq = unSavedItem.Freq;
                savedItem.Interval = unSavedItem.Interval;
                savedItem.ValidFrom = unSavedItem.ValidFrom;
                savedItem.ValidTo = unSavedItem.ValidTo;
            }

            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<PhysicalAddress> AttachExistingPhysicalAddress(ICollection<PhysicalAddressDto>? unSavedEntities, string locationId)
    {
        var returnList = new List<PhysicalAddress>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.PhysicalAddresses.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = _mapper.Map<PhysicalAddress>(unSavedEntities.ElementAt(i));
            unSavedItem.LocationId = locationId;

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            
            if (savedItem is not null)
            {
                savedItem.Address1 = unSavedItem.Address1;
                savedItem.City = unSavedItem.City;
                savedItem.Country = unSavedItem.Country;
                savedItem.StateProvince = unSavedItem.StateProvince;
                savedItem.PostCode = unSavedItem.PostCode;
            }

            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }

    private ICollection<LinkTaxonomy> AttachExistingLinkTaxonomy(ICollection<LinkTaxonomyDto>? unSavedEntities, string? serviceId, string? serviceAtLocationId, string? locationId)
    {
        var returnList = new List<LinkTaxonomy>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.LinkTaxonomies
            .Include(t => t.Taxonomy)
            .Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        var newIds = unSavedEntities.Where(c => c.Taxonomy != null).Select(c => c.Taxonomy!.Id).ToList();
        var existingChilds = _context.Taxonomies.Where(x => newIds.Contains(x.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {

            var unSavedItem = _mapper.Map<LinkTaxonomy>(unSavedEntities.ElementAt(i));

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            var itemToSave = savedItem ?? _mapper.Map<LinkTaxonomy>(unSavedItem);
            var linkId = serviceId ?? serviceAtLocationId ?? locationId;
            ArgumentException.ThrowIfNullOrEmpty(linkId);
            itemToSave.LinkId = linkId;

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

    private ICollection<LinkContact> AttachExistingContacts(ICollection<LinkContactDto>? unSavedEntities, string? serviceId, string? serviceAtLocationId, string? locationId)
    {
        var returnList = new List<LinkContact>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.LinkContacts
            .Include(t => t.Contact)
            .Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        var newIds = unSavedEntities.Select(c => c.Contact.Id).ToList();
        var existingChilds = _context.Contacts.Where(x => newIds.Contains(x.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            var itemToSave = savedItem ?? _mapper.Map<LinkContact>(unSavedItem);
            var linkId = serviceId ?? serviceAtLocationId ?? locationId;
            ArgumentException.ThrowIfNullOrEmpty(linkId);
            itemToSave.LinkId = linkId;

            if (itemToSave.Contact != null)
            {
                var existingContact = existing.Select(t => t.Contact).SingleOrDefault(t => t!.Id == itemToSave.Contact.Id)
                                      ?? existingChilds.SingleOrDefault(t => t.Id == itemToSave.Contact.Id);

                if (existingContact is not null)
                {
                    var unsavedContact = unSavedItem.Contact;

                    itemToSave.Contact = existingContact;

                    itemToSave.Contact.Title = unsavedContact.Title;
                    itemToSave.Contact.Name = unsavedContact.Name;
                    itemToSave.Contact.Email = unsavedContact.Email;
                    itemToSave.Contact.Telephone = unsavedContact.Telephone;
                    itemToSave.Contact.TextPhone = unsavedContact.TextPhone;
                    itemToSave.Contact.Url = unsavedContact.Url;
                }
            }

            returnList.Add(itemToSave);
        }

        return returnList;
    }
}