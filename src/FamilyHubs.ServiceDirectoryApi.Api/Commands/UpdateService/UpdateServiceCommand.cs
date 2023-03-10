using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;

public class UpdateServiceCommand : IRequest<long>
{
    public UpdateServiceCommand(long id, ServiceDto service)
    {
        Id = id;
        Service = service;
    }

    public ServiceDto Service { get; }

    public long Id { get; }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateServiceCommandHandler> _logger;
    private readonly IMapper _mapper;
    private List<Contact> _dbContacts = new List<Contact>();

    public UpdateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.Services.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        _dbContacts = await _context.Contacts.ToListAsync(cancellationToken);

        try
        {
            var serviceEntity = _mapper.Map<Service>(request.Service);
            ArgumentNullException.ThrowIfNull(serviceEntity);

            entity.ServiceType = serviceEntity.ServiceType;
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
            entity.Contacts = AttachExistingContacts(request.Service.Contacts);
            entity.Languages = AttachExistingLanguages(request.Service.Languages);
            entity.Locations = AttachExistingLocation(request.Service.Locations);
            entity.Taxonomies = AttachExistingServiceTaxonomies(request.Service.Taxonomies);
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
                savedItem.EligibilityType = unSavedItem.EligibilityType;
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
                savedItem.Option = unSavedItem.Option ?? string.Empty;
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
                savedItem.ServiceAreaName = unSavedItem.ServiceAreaName;
                savedItem.Uri = unSavedItem.Uri;
            }

            returnList.Add(savedItem ?? _mapper.Map<ServiceArea>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<Taxonomy> AttachExistingServiceTaxonomies(ICollection<TaxonomyDto>? unSavedEntities)
    {
        var returnList = new List<Taxonomy>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Taxonomies.Where(e => unSavedEntities.Select(c => c.Name).Contains(e.Name)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);
            var itemToSave = savedItem ?? _mapper.Map<Taxonomy>(unSavedItem);
            returnList.Add(itemToSave);
        }

        return returnList;
    }

    private ICollection<Location> AttachExistingLocation(ICollection<LocationDto>? unSavedEntities)
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

        for (int i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.Name = unSavedItem.Name;
                savedItem.Description = unSavedItem.Description;
                savedItem.Latitude = unSavedItem.Latitude;
                savedItem.Longitude = unSavedItem.Longitude;
                //savedItem.Address1 = unSavedItem.Address1;
                //savedItem.City = unSavedItem.City;
                //savedItem.Country = unSavedItem.Country;
                //savedItem.StateProvince = unSavedItem.StateProvince;
                //savedItem.PostCode = unSavedItem.PostCode;
            }

            var returnItem = savedItem ?? _mapper.Map<Location>(unSavedItem);

            //returnItem.Contacts = AttachExistingContacts(unSavedItem.Contacts, null, null, returnItem.Id);
            //returnItem.HolidaySchedules = AttachExistingHolidaySchedule(unSavedItem.HolidaySchedules, null, returnItem.Id);
            //returnItem.RegularSchedules = AttachExistingRegularSchedule(unSavedItem.RegularSchedules, null, returnItem.Id);

            returnList.Add(returnItem);
        }

        return returnList;
    }

    private ICollection<HolidaySchedule> AttachExistingHolidaySchedule(ICollection<HolidayScheduleDto>? unSavedEntities, long? serviceId, long? locationId)
    {
        var returnList = new List<HolidaySchedule>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.HolidaySchedules.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = _mapper.Map<HolidaySchedule>(unSavedEntities.ElementAt(i));
            unSavedItem.ServiceId = serviceId!;
            unSavedItem.LocationId = locationId!;

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

    private ICollection<RegularSchedule> AttachExistingRegularSchedule(ICollection<RegularScheduleDto>? unSavedEntities, long? serviceId, long? locationId)
    {
        var returnList = new List<RegularSchedule>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.RegularSchedules.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = _mapper.Map<RegularSchedule>(unSavedEntities.ElementAt(i));
            unSavedItem.ServiceId = serviceId!;
            unSavedItem.LocationId = locationId!;

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

    private ICollection<Contact> AttachExistingContacts(ICollection<ContactDto>? unSavedEntities)
    {
        var returnList = new List<Contact>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = (from dbContact in _dbContacts
                join newContact in unSavedEntities on
                    new { dbContact.Name, dbContact.Email, dbContact.Telephone } equals 
                    new { newContact.Name, newContact.Email, newContact.Telephone }
                select dbContact)
            .ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);

            var savedItem = existing.FirstOrDefault(x => x.Id == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.Title = unSavedItem.Title;
                savedItem.Name = unSavedItem.Name;
                savedItem.Email = unSavedItem.Email;
                savedItem.Telephone = unSavedItem.Telephone;
                savedItem.TextPhone = unSavedItem.TextPhone;
                savedItem.Url = unSavedItem.Url;
            }

            var itemToSave = savedItem ?? _mapper.Map<Contact>(unSavedItem);

            returnList.Add(itemToSave);
        }

        return returnList;
    }
}