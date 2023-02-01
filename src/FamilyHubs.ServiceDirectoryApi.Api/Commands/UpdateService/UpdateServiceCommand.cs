using System.Collections.ObjectModel;
using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
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

    public ServiceDto Service { get; init; }

    public string Id { get; set; }
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
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _request = request;

        var entity = await _context.Services
           .Include(x => x.ServiceType)
           .Include(x => x.ServiceDeliveries)
           .Include(x => x.Eligibilities)
           .Include(x => x.LinkContacts)
           .Include(x => x.CostOptions)
           .Include(x => x.Languages)
           .Include(x => x.ServiceAreas)

           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.PhysicalAddresses)

           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.LinkTaxonomies!)
           .ThenInclude(x => x.Taxonomy)

           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.RegularSchedules)

           .Include(x => x.ServiceAtLocations)
           .ThenInclude(x => x.HolidaySchedules)

           .Include(x => x.RegularSchedules)
           .Include(x => x.HolidaySchedules)

           .Include(x => x.ServiceTaxonomies)
           .ThenInclude(x => x.Taxonomy)
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Service), request.Id);
        }

        try
        {
            var serviceEntity = _mapper.Map<Service>(request.Service);
            ArgumentNullException.ThrowIfNull(serviceEntity, nameof(serviceEntity));

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

            foreach (var serviceAtLocation in serviceEntity.ServiceAtLocations)
            {
                if (serviceAtLocation.RegularSchedules != null)
                {
                    foreach (var regularSchedules in serviceAtLocation.RegularSchedules)
                    {
                        regularSchedules.ServiceAtLocationId = serviceAtLocation.Id;
                    }
                }

                if (serviceAtLocation.HolidaySchedules != null)
                {
                    foreach (var holidaySchedules in serviceAtLocation.HolidaySchedules)
                    {
                        holidaySchedules.ServiceAtLocationId = serviceAtLocation.Id;
                    }
                }
            }

            if (request.Service.Eligibilities != null && entity.Eligibilities.Serialize() != request.Service.Eligibilities.Serialize())
                UpdateEligibility(entity.Eligibilities, request.Service.Eligibilities ?? new Collection<EligibilityDto>());
            if (request.Service.ServiceAreas != null && entity.ServiceAreas.Serialize() != request.Service.ServiceAreas.Serialize())
                UpdateServiceArea(entity.ServiceAreas, request.Service.ServiceAreas ?? new Collection<ServiceAreaDto>());
            if (request.Service.ServiceDeliveries != null && entity.ServiceDeliveries.Serialize() != request.Service.ServiceDeliveries.Serialize())
                UpdateServiceDelivery(entity.ServiceDeliveries, request.Service.ServiceDeliveries ?? new Collection<ServiceDeliveryDto>());
            if (request.Service.LinkContacts != null && entity.LinkContacts.Serialize() != request.Service.LinkContacts.Serialize())
                UpdateContacts(entity.LinkContacts, request.Service.LinkContacts ?? new Collection<LinkContactDto>());
            if (request.Service.Languages != null && entity.Languages.Serialize() != request.Service.Languages.Serialize())
                UpdateLanguages(entity.Languages, request.Service.Languages ?? new Collection<LanguageDto>());
            if (request.Service.ServiceAtLocations != null && entity.ServiceAtLocations.Serialize() != request.Service.ServiceAtLocations.Serialize())
                UpdateServiceAtLocation(entity.ServiceAtLocations, request.Service.ServiceAtLocations ?? new Collection<ServiceAtLocationDto>());
            if (request.Service.ServiceTaxonomies != null && entity.ServiceTaxonomies.Serialize() != request.Service.ServiceTaxonomies.Serialize())
                UpdateTaxonomies(entity.ServiceTaxonomies, request.Service.ServiceTaxonomies ?? new Collection<ServiceTaxonomyDto>());
            if (request.Service.CostOptions != null && entity.CostOptions.Serialize() != request.Service.CostOptions.Serialize())
                UpdateCostOptions(entity.CostOptions, request.Service.CostOptions ?? new Collection<CostOptionDto>());
            if (request.Service.RegularSchedules != null && entity.RegularSchedules.Serialize() != request.Service.RegularSchedules.Serialize())
                UpdateRegularSchedule(entity.RegularSchedules, request.Service.RegularSchedules ?? new Collection<RegularScheduleDto>(), null);
            if (request.Service.HolidaySchedules != null && entity.HolidaySchedules.Serialize() != request.Service.HolidaySchedules.Serialize())
                UpdateHolidaySchedule(entity.HolidaySchedules, request.Service.HolidaySchedules ?? new Collection<HolidayScheduleDto>(), null);

            await _context.SaveChangesAsync(cancellationToken);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }

    private void UpdateEligibility(ICollection<Eligibility> existing, ICollection<EligibilityDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedEligibility in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedEligibility.Id);
            if (current == null)
            {
                var entity = _mapper.Map<Eligibility>(updatedEligibility);
                entity.ServiceId = _request.Service.Id;
                entity.RegisterDomainEvent(new EligibilityCreatedEvent(entity));
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

    private void UpdateServiceAtLocation(ICollection<ServiceAtLocation> existing, ICollection<ServiceAtLocationDto> updated)
    {
        List<string> list = new();
        List<string> listAddress = new();

        foreach (var updatedServiceLoc in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceLoc.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceAtLocation>(updatedServiceLoc);
                entity.ServiceId = _request.Service.Id;
                entity.RegisterDomainEvent(new ServiceAtLocationCreatedEvent(entity));
                _context.ServiceAtLocations.Add(entity);
                list.Add(entity.Id);
                if (entity.Location.PhysicalAddresses != null)
                {
                    foreach (var address in entity.Location.PhysicalAddresses)
                    {
                        address.LocationId = entity.Location.Id;
                        listAddress.Add(address.Id);
                    }
                }
            }
            else
            {
                current.Location.Name = updatedServiceLoc.Location.Name;
                current.Location.Description = updatedServiceLoc.Location.Description;
                current.Location.Latitude = updatedServiceLoc.Location.Latitude;
                current.Location.Longitude = updatedServiceLoc.Location.Longitude;

                if (updatedServiceLoc.Location.PhysicalAddresses != null)
                {
                    foreach (var address in updatedServiceLoc.Location.PhysicalAddresses)
                    {
                        var currentAddress = _context.PhysicalAddresses.FirstOrDefault(x => x.Id == address.Id);
                        if (currentAddress == null)
                        {
                            var entity = _mapper.Map<PhysicalAddress>(address);
                            entity.LocationId = updatedServiceLoc.Location.Id;
                            entity.RegisterDomainEvent(new PhysicalAddressCreatedEvent(entity));
                            _context.PhysicalAddresses.Add(entity);
                            listAddress.Add(entity.Id);
                        }
                        else
                        {
                            currentAddress.LocationId = updatedServiceLoc.Location.Id;
                            currentAddress.Address1 = address.Address1;
                            currentAddress.City = address.City;
                            currentAddress.PostCode = address.PostCode;
                            currentAddress.Country = address.Country;
                            currentAddress.StateProvince = address.StateProvince;
                            listAddress.Add(currentAddress.Id);
                        }
                    }
                    if (updatedServiceLoc.RegularSchedules != null && current.RegularSchedules != null && current.RegularSchedules.Serialize() != updatedServiceLoc.RegularSchedules.Serialize())
                    {
                        UpdateRegularSchedule(current.RegularSchedules ?? new Collection<RegularSchedule>(), updatedServiceLoc.RegularSchedules ?? new Collection<RegularScheduleDto>(), current);
                    }
                    if (current.HolidaySchedules != null && updatedServiceLoc.HolidaySchedules != null && current.HolidaySchedules.Serialize() != updatedServiceLoc.HolidaySchedules.Serialize())
                    {
                        UpdateHolidaySchedule(current.HolidaySchedules ?? new Collection<HolidaySchedule>(), updatedServiceLoc.HolidaySchedules ?? new Collection<HolidayScheduleDto>(), current);
                    }
                }
                if (updatedServiceLoc.Location.LinkTaxonomies != null && updatedServiceLoc.Location.LinkTaxonomies.Any())
                {
                    foreach (var linkTaxonomyDto in updatedServiceLoc.Location.LinkTaxonomies)
                    {
                        var linkTaxonomy =  _context.LinkTaxonomies.SingleOrDefault(p => p.Id == linkTaxonomyDto.Id);
                        if (linkTaxonomy != null)
                        {
                            linkTaxonomy.LinkType = linkTaxonomyDto.LinkType;
                            linkTaxonomy.LinkId = linkTaxonomyDto.LinkId;

                            if (linkTaxonomyDto.Taxonomy != null)
                            {
                                var taxonomy = _context.Taxonomies.FirstOrDefault(x => linkTaxonomy.Taxonomy != null && x.Id == linkTaxonomy.Taxonomy.Id);
                                if (taxonomy != null)
                                {
                                    linkTaxonomy.Taxonomy = taxonomy;
                                }
                            }
                        }
                        else
                        {
                            var linkTaxonomyEntity = _mapper.Map<LinkTaxonomy>(linkTaxonomyDto);

                            if (linkTaxonomyEntity.Taxonomy != null)
                            {
                                var taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == linkTaxonomyEntity.Taxonomy.Id);
                                if (taxonomy != null)
                                {
                                    linkTaxonomyEntity.Taxonomy = taxonomy;
                                }
                            }

                            ArgumentNullException.ThrowIfNull(linkTaxonomyEntity, nameof(linkTaxonomyEntity));

                            linkTaxonomyEntity.RegisterDomainEvent(new LinkTaxonomyCreatedEvent(linkTaxonomyEntity));

                             _context.LinkTaxonomies.Add(linkTaxonomyEntity);
                        }
                    }
                }

                list.Add(current.Id);
            }
        }

        foreach (var item in existing)
        {
            if (item.Location.PhysicalAddresses != null)
            {
                foreach (var address in item.Location.PhysicalAddresses)
                {
                    if (!listAddress.Contains(address.Id))
                    {
                        _context.PhysicalAddresses.Remove(address);
                    }
                }
            }
            

            if (!list.Contains(item.Id))
            {
                _context.ServiceAtLocations.Remove(item);
            }
        }
        
    }

    private void UpdateHolidaySchedule(ICollection<HolidaySchedule> existing, ICollection<HolidayScheduleDto> updated, ServiceAtLocation? serviceAtLocation)
    {
        List<string> currentIds = new();
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<HolidaySchedule>(updatedSchedule);
                if (serviceAtLocation != null)
                    entity.ServiceAtLocationId = serviceAtLocation.Id;
                entity.RegisterDomainEvent(new HolidayScheduleCreatedEvent(entity));
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

    private void UpdateRegularSchedule(ICollection<RegularSchedule> existing, ICollection<RegularScheduleDto> updated, ServiceAtLocation? serviceAtLocation)
    {
        List<string> currentIds = new();
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<RegularSchedule>(updatedSchedule);
                if (serviceAtLocation != null)
                    entity.ServiceAtLocationId = serviceAtLocation.Id;
                entity.RegisterDomainEvent(new RegularScheduleCreatedEvent(entity));
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

    private void UpdateServiceArea(ICollection<ServiceArea> existing, ICollection<ServiceAreaDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedServiceArea in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceArea.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceArea>(updatedServiceArea);
                entity.ServiceId = _request.Service.Id;
                entity.RegisterDomainEvent(new ServiceAreaCreatedEvent(entity));
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

    private void UpdateCostOptions(ICollection<CostOption> existing, ICollection<CostOptionDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedCostOption in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedCostOption.Id);
            if (current == null)
            {
                var entity = _mapper.Map<CostOption>(updatedCostOption);
                entity.ServiceId = _request.Service.Id;
                entity.RegisterDomainEvent(new CostOptionCreatedEvent(entity));
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

    private void UpdateTaxonomies(ICollection<ServiceTaxonomy> existing, ICollection<ServiceTaxonomyDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedServiceTaxonomy in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceTaxonomy>(updatedServiceTaxonomy);
                if (updatedServiceTaxonomy.Taxonomy != null)
                    entity.Taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Taxonomy.Id);
                entity.ServiceId = _request.Service.Id;
                entity.RegisterDomainEvent(new ServiceTaxonomyCreatedEvent(entity));
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

    private void UpdateLanguages(ICollection<Language> existing, ICollection<LanguageDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedLanguage in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedLanguage.Id);
            if (current == null)
            {
                var entity = _mapper.Map<Language>(updatedLanguage);
                entity.ServiceId = _request.Service.Id;
                entity.RegisterDomainEvent(new LanguageCreatedEvent(entity));
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

    

    private void UpdateContacts(ICollection<LinkContact> existing, ICollection<LinkContactDto> updated)
    {
        foreach (var updatedContact in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedContact.Id);
            if (current == null)
            {
                var entity = _mapper.Map<LinkContact>(updatedContact);
                entity.LinkId = _request.Service.Id;
                _context.LinkContacts.Add(entity);
            }
            else if (current.Contact != null)
            {
                current.Contact.Title     = updatedContact.Contact.Title;
                current.Contact.Name      = updatedContact.Contact.Name;
                current.Contact.Telephone = updatedContact.Contact.Telephone;
                current.Contact.TextPhone = updatedContact.Contact.TextPhone;
                current.Contact.Url       = updatedContact.Contact.Url;
                current.Contact.Email     = updatedContact.Contact.Email;
            }
        }

        var contactToDelete = existing.Where(a => !existing.Select(x => x.Id).Contains(a.Id)).ToList();
        if (contactToDelete.Any())
        {
            _context.LinkContacts.RemoveRange(contactToDelete);
        }
    }

    private void UpdateServiceDelivery(ICollection<ServiceDelivery> existing, ICollection<ServiceDeliveryDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedServiceDelivery in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
            if (current == null)
            {
                var entity = _mapper.Map<ServiceDelivery>(updatedServiceDelivery);
                entity.ServiceId = _request.Service.Id;
                entity.RegisterDomainEvent(new ServiceDeliveryCreatedEvent(entity));
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


