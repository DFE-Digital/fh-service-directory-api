using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core;
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
    private UpdateServiceCommand _request = default!;

    public UpdateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _request = request;

        var entity = await _context.Services
            .Include(x => x.ServiceType)
            .Include(x => x.ServiceDeliveries)
            .Include(x => x.Eligibilities)
            .Include(x => x.CostOptions)
            .Include(x => x.Fundings)
            .Include(x => x.Languages)
            .Include(x => x.ServiceAreas)
            .Include(x => x.RegularSchedules)
            .Include(x => x.HolidaySchedules)
            .Include(x => x.LinkContacts)
            .ThenInclude(x => x.Contact)
            .Include(x => x.ServiceTaxonomies)
            .ThenInclude(x => x.Taxonomy)

            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.RegularSchedules)
            
            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.LinkContacts!)
            .ThenInclude(x => x.Contact)
            
            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.PhysicalAddresses)
            
            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.LinkContacts!)
            .ThenInclude(x => x.Contact)
            
            .Include(x => x.ServiceAtLocations)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.LinkTaxonomies!.Where(lt => lt.LinkType == LinkType.Location))
            .ThenInclude(x => x.Taxonomy)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

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

            UpdateEligibility(entity.Eligibilities, request.Service.Eligibilities);
            UpdateServiceArea(entity.ServiceAreas, request.Service.ServiceAreas);
            UpdateServiceDelivery(entity.ServiceDeliveries, request.Service.ServiceDeliveries);
            UpdateContacts(entity.LinkContacts, request.Service.LinkContacts, entity.Id);
            UpdateLanguages(entity.Languages, request.Service.Languages);
            UpdateServiceAtLocation(entity.ServiceAtLocations, request.Service.ServiceAtLocations);
            UpdateTaxonomies(entity.ServiceTaxonomies, request.Service.ServiceTaxonomies);
            UpdateCostOptions(entity.CostOptions, request.Service.CostOptions);
            UpdateRegularSchedule(entity.RegularSchedules, request.Service.RegularSchedules, entity.Id, string.Empty);
            UpdateHolidaySchedule(entity.HolidaySchedules, request.Service.HolidaySchedules, entity.Id, string.Empty);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw;
        }

        return entity.Id;
    }

    private void UpdateEligibility(ICollection<Eligibility> existing, ICollection<EligibilityDto>? updated)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedEligibility in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedEligibility.Id);
            if (current == null)
            {
                var entity = _mapper.Map<Eligibility>(updatedEligibility);
                entity.ServiceId = _request.Service.Id;
                _context.Eligibilities.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                currentIds.Add(updatedEligibility.Id);
                current.EligibilityDescription = updatedEligibility.EligibilityDescription;
                current.MaximumAge = updatedEligibility.MaximumAge;
                current.MinimumAge = updatedEligibility.MinimumAge;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.Eligibilities.RemoveRange(dataToDelete);
    }

    private void UpdateServiceAtLocation(ICollection<ServiceAtLocation> existing, ICollection<ServiceAtLocationDto>? updated)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();

        foreach (var updatedServiceLoc in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceLoc.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceAtLocation>(updatedServiceLoc);
                entity.ServiceId = _request.Service.Id;
                entity.HolidaySchedules?.Clear();
                entity.RegularSchedules?.Clear();
                entity.LinkContacts?.Clear();

                entity.Location.LinkContacts?.Clear();
                entity.Location.AccessibilityForDisabilities?.Clear();
                entity.Location.LinkTaxonomies?.Clear();
                entity.Location.PhysicalAddresses?.Clear();

                UpdateServiceAtLocationChildEntities(entity, updatedServiceLoc);

                _context.ServiceAtLocations.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Location.Name = updatedServiceLoc.Location.Name;
                current.Location.Description = updatedServiceLoc.Location.Description;
                current.Location.Latitude = updatedServiceLoc.Location.Latitude;
                current.Location.Longitude = updatedServiceLoc.Location.Longitude;

                UpdateServiceAtLocationChildEntities(current, updatedServiceLoc);

                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.ServiceAtLocations.RemoveRange(dataToDelete);
    }

    private void UpdateServiceAtLocationChildEntities(ServiceAtLocation existing, ServiceAtLocationDto updated)
    {
        //Update Service at Location Level data
        UpdateContacts(existing.LinkContacts ?? new List<LinkContact>(), updated.LinkContacts, existing.Id);

        UpdateRegularSchedule(existing.RegularSchedules ?? new List<RegularSchedule>(), updated.RegularSchedules, string.Empty, existing.Id);

        UpdateHolidaySchedule(existing.HolidaySchedules ?? new List<HolidaySchedule>(), updated.HolidaySchedules, string.Empty, existing.Id);

        //Update Location Level Data
        UpdatePhysicalAddress(existing.Location.PhysicalAddresses ?? new List<PhysicalAddress>(), updated.Location.PhysicalAddresses, existing.Location);

        UpdateLinkTaxonomy(existing.Location.LinkTaxonomies ?? new List<LinkTaxonomy>(), updated.Location.LinkTaxonomies, existing.Location);

        UpdateContacts(existing.Location.LinkContacts ?? new List<LinkContact>(), updated.Location.LinkContacts, existing.Location.Id);
    }

    private void UpdatePhysicalAddress(ICollection<PhysicalAddress> existing, ICollection<PhysicalAddressDto>? updated, Location parentLocation)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();

        foreach (var address in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == address.Id);
            if (current == null)
            {
                var entity = _mapper.Map<PhysicalAddress>(address);
                entity.LocationId = parentLocation.Id;
                _context.PhysicalAddresses.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.LocationId = parentLocation.Id;
                current.Address1 = address.Address1;
                current.City = address.City;
                current.PostCode = address.PostCode;
                current.Country = address.Country;
                current.StateProvince = address.StateProvince;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.PhysicalAddresses.RemoveRange(dataToDelete);
    }

    private void UpdateLinkTaxonomy(ICollection<LinkTaxonomy> existing, ICollection<LinkTaxonomyDto>? updated, Location parentLocation)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();

        foreach (var updatedLinkTaxonomy in updated)
        {
            var existingLinkTaxonomy = existing.SingleOrDefault(p => p.Id == updatedLinkTaxonomy.Id);
            var taxonomy = _context.Taxonomies.FirstOrDefault(t => updatedLinkTaxonomy.Taxonomy != null && t.Id == updatedLinkTaxonomy.Taxonomy.Id);

            if (existingLinkTaxonomy == null)
            {
                var newLinkTaxonomy = _mapper.Map<LinkTaxonomy>(updatedLinkTaxonomy);
                newLinkTaxonomy.LinkId = parentLocation.Id;

                if (updatedLinkTaxonomy.Taxonomy != null && newLinkTaxonomy.Taxonomy != null
                    && updatedLinkTaxonomy.Taxonomy.Id != newLinkTaxonomy.Taxonomy.Id)
                {
                    newLinkTaxonomy.Taxonomy = taxonomy;
                }

                ArgumentNullException.ThrowIfNull(newLinkTaxonomy);

                _context.LinkTaxonomies.Add(newLinkTaxonomy);

                currentIds.Add(newLinkTaxonomy.Id);
            }
            else
            {
                existingLinkTaxonomy.LinkType = updatedLinkTaxonomy.LinkType;
                existingLinkTaxonomy.LinkId = updatedLinkTaxonomy.LinkId;

                if (updatedLinkTaxonomy.Taxonomy != null && existingLinkTaxonomy.Taxonomy != null
                    && updatedLinkTaxonomy.Taxonomy.Id != existingLinkTaxonomy.Taxonomy.Id)
                {
                    existingLinkTaxonomy.Taxonomy = taxonomy;
                }

                currentIds.Add(existingLinkTaxonomy.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.LinkTaxonomies.RemoveRange(dataToDelete);
    }

    private void UpdateHolidaySchedule(ICollection<HolidaySchedule> existing, ICollection<HolidayScheduleDto>? updated, string serviceId, string serviceAtLocationId)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<HolidaySchedule>(updatedSchedule);
                entity.ServiceId = serviceId;
                entity.ServiceAtLocationId = serviceAtLocationId;
                _context.HolidaySchedules.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Closed = updatedSchedule.Closed;
                current.ClosesAt = updatedSchedule.ClosesAt;
                current.StartDate = updatedSchedule.StartDate;
                current.EndDate = updatedSchedule.EndDate;
                current.OpensAt = updatedSchedule.OpensAt;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.HolidaySchedules.RemoveRange(dataToDelete);
    }

    private void UpdateRegularSchedule(ICollection<RegularSchedule> existing, ICollection<RegularScheduleDto>? updated, string serviceId, string serviceAtLocationId)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<RegularSchedule>(updatedSchedule);
                entity.ServiceId = serviceId;
                entity.ServiceAtLocationId = serviceAtLocationId;
                _context.RegularSchedules.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Description = updatedSchedule.Description;
                current.OpensAt = updatedSchedule.OpensAt;
                current.ClosesAt = updatedSchedule.ClosesAt;
                current.ByDay = updatedSchedule.ByDay;
                current.ByMonthDay = updatedSchedule.ByMonthDay;
                current.DtStart = updatedSchedule.DtStart;
                current.Freq = updatedSchedule.Freq;
                current.Interval = updatedSchedule.Interval;
                current.ValidFrom = updatedSchedule.ValidFrom;
                current.ValidTo = updatedSchedule.ValidTo;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.RegularSchedules.RemoveRange(dataToDelete);
    }

    private void UpdateServiceArea(ICollection<ServiceArea> existing, ICollection<ServiceAreaDto>? updated)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedServiceArea in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceArea.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceArea>(updatedServiceArea);
                entity.ServiceId = _request.Service.Id;
                _context.ServiceAreas.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.ServiceAreaDescription = updatedServiceArea.ServiceAreaDescription;
                current.Extent = updatedServiceArea.Extent;
                current.Uri = updatedServiceArea.Uri;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.ServiceAreas.RemoveRange(dataToDelete);
    }

    private void UpdateCostOptions(ICollection<CostOption> existing, ICollection<CostOptionDto>? updated)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedCostOption in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedCostOption.Id);
            if (current == null)
            {
                var entity = _mapper.Map<CostOption>(updatedCostOption);
                entity.ServiceId = _request.Service.Id;
                _context.CostOptions.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.LinkId = updatedCostOption.LinkId;
                current.Amount = updatedCostOption.Amount;
                current.AmountDescription = updatedCostOption.AmountDescription;
                current.Option = updatedCostOption.Option;
                current.ValidFrom = updatedCostOption.ValidFrom;
                current.ValidTo = updatedCostOption.ValidTo;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.CostOptions.RemoveRange(dataToDelete);

    }

    private void UpdateTaxonomies(ICollection<ServiceTaxonomy> existing, ICollection<ServiceTaxonomyDto>? updated)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedServiceTaxonomy in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceTaxonomy>(updatedServiceTaxonomy);
                if (updatedServiceTaxonomy.Taxonomy != null)
                    entity.Taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Taxonomy.Id);
                entity.ServiceId = _request.Service.Id;
                _context.ServiceTaxonomies.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.ServiceTaxonomies.RemoveRange(dataToDelete);
    }

    private void UpdateLanguages(ICollection<Language> existing, ICollection<LanguageDto>? updated)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedLanguage in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedLanguage.Id);
            if (current == null)
            {
                var entity = _mapper.Map<Language>(updatedLanguage);
                entity.ServiceId = _request.Service.Id;
                _context.Languages.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Name = updatedLanguage.Name;
                currentIds.Add(current.Id);
            }
        }
        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.Languages.RemoveRange(dataToDelete);
    }

    private void UpdateContacts(ICollection<LinkContact> existing, ICollection<LinkContactDto>? updated, string parentId)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();

        foreach (var updatedContact in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedContact.Id);
            if (current == null)
            {
                var entity = _mapper.Map<LinkContact>(updatedContact);
                entity.LinkId = parentId;

                if (entity.Contact != null)
                {
                    var contact = _context.Contacts.FirstOrDefault(x => x.Id == entity.Contact.Id);
                    if (contact != null)
                    {
                        entity.Contact = contact;
                    }
                }

                _context.LinkContacts.Add(entity);

                currentIds.Add(entity.Id);
            }
            else if (current.Contact != null)
            {
                current.Contact.Title = updatedContact.Contact.Title;
                current.Contact.Name = updatedContact.Contact.Name;
                current.Contact.Telephone = updatedContact.Contact.Telephone;
                current.Contact.TextPhone = updatedContact.Contact.TextPhone;
                current.Contact.Url = updatedContact.Contact.Url;
                current.Contact.Email = updatedContact.Contact.Email;

                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.LinkContacts.RemoveRange(dataToDelete);
    }

    private void UpdateServiceDelivery(ICollection<ServiceDelivery> existing, ICollection<ServiceDeliveryDto>? updated)
    {
        if (updated is null || !updated.Any())
            return;

        var currentIds = new List<string>();
        foreach (var updatedServiceDelivery in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceDelivery>(updatedServiceDelivery);
                entity.ServiceId = _request.Service.Id;
                _context.ServiceDeliveries.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Name = updatedServiceDelivery.Name;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.ServiceDeliveries.RemoveRange(dataToDelete);
    }
}