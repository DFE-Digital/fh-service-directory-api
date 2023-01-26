using System.Collections.ObjectModel;
using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralHolidaySchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralRegularSchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralService;

public class UpdateOpenReferralServiceCommand : IRequest<string>
{
    public UpdateOpenReferralServiceCommand(string id, OpenReferralServiceDto openReferralService)
    {
        Id = id;
        OpenReferralService = openReferralService;
    }

    public OpenReferralServiceDto OpenReferralService { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralServiceCommandHandler : IRequestHandler<UpdateOpenReferralServiceCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOpenReferralServiceCommandHandler> _logger;
    private readonly IMapper _mapper;
    private UpdateOpenReferralServiceCommand _request = default!;

    public UpdateOpenReferralServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateOpenReferralServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _request = request;

        var entity = await _context.OpenReferralServices
           .Include(x => x.ServiceType)
           .Include(x => x.ServiceDelivery)
           .Include(x => x.Eligibilities)
           .Include(x => x.Contacts)
           .Include(x => x.Cost_options)
           .Include(x => x.Languages)
           .Include(x => x.Service_areas)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.Physical_addresses)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.LinkTaxonomies!)
           .ThenInclude(x => x.Taxonomy)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Regular_schedule)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.HolidayScheduleCollection)

           .Include(x => x.Regular_schedules)
           .Include(x => x.Holiday_schedules)

           .Include(x => x.Service_taxonomys)
           .ThenInclude(x => x.Taxonomy)
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        try
        {
            var serviceEntity = _mapper.Map<OpenReferralService>(request.OpenReferralService);
            ArgumentNullException.ThrowIfNull(serviceEntity, nameof(serviceEntity));

            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.OpenReferralService.ServiceType.Id);
            if (serviceType != null)
                entity.ServiceType = serviceType;

            entity.Name = serviceEntity.Name;
            entity.Description = serviceEntity.Description;
            entity.Accreditations = serviceEntity.Accreditations;
            entity.Assured_date = serviceEntity.Assured_date;
            entity.Attending_access = serviceEntity.Attending_access;
            entity.Attending_type = serviceEntity.Attending_type;
            entity.Deliverable_type = serviceEntity.Deliverable_type;
            entity.Status = serviceEntity.Status;
            entity.Url = serviceEntity.Url;
            entity.Email = serviceEntity.Email;
            entity.Fees = serviceEntity.Fees;

            foreach (var serviceAtLocation in serviceEntity.Service_at_locations)
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
            }

            if (request.OpenReferralService.Eligibilities != null && entity.Eligibilities.Serialize() != request.OpenReferralService.Eligibilities.Serialize())
                UpdateEligibility(entity.Eligibilities, request.OpenReferralService.Eligibilities ?? new Collection<OpenReferralEligibilityDto>());
            if (request.OpenReferralService.Service_areas != null && entity.Service_areas.Serialize() != request.OpenReferralService.Service_areas.Serialize())
                UpdateServiceArea(entity.Service_areas, request.OpenReferralService.Service_areas ?? new Collection<OpenReferralServiceAreaDto>());
            if (request.OpenReferralService.ServiceDelivery != null && entity.ServiceDelivery.Serialize() != request.OpenReferralService.ServiceDelivery.Serialize())
                UpdateServiceDelivery(entity.ServiceDelivery, request.OpenReferralService.ServiceDelivery ?? new Collection<OpenReferralServiceDeliveryExDto>());
            if (request.OpenReferralService.Contacts != null && entity.Contacts.Serialize() != request.OpenReferralService.Contacts.Serialize())
                UpdateContacts(entity.Contacts, request.OpenReferralService.Contacts ?? new Collection<OpenReferralContactDto>());
            if (request.OpenReferralService.Languages != null && entity.Languages.Serialize() != request.OpenReferralService.Languages.Serialize())
                UpdateLanguages(entity.Languages, request.OpenReferralService.Languages ?? new Collection<OpenReferralLanguageDto>());
            if (request.OpenReferralService.Service_at_locations != null && entity.Service_at_locations.Serialize() != request.OpenReferralService.Service_at_locations.Serialize())
                UpdateServiceAtLocation(entity.Service_at_locations, request.OpenReferralService.Service_at_locations ?? new Collection<OpenReferralServiceAtLocationDto>());
            if (request.OpenReferralService.Service_taxonomys != null && entity.Service_taxonomys.Serialize() != request.OpenReferralService.Service_taxonomys.Serialize())
                UpdateTaxonomies(entity.Service_taxonomys, request.OpenReferralService.Service_taxonomys ?? new Collection<OpenReferralServiceTaxonomyDto>());
            if (request.OpenReferralService.Cost_options != null && entity.Cost_options.Serialize() != request.OpenReferralService.Cost_options.Serialize())
                UpdateCostOptions(entity.Cost_options, request.OpenReferralService.Cost_options ?? new Collection<OpenReferralCostOptionDto>());
            if (request.OpenReferralService.RegularSchedules != null && entity.Regular_schedules.Serialize() != request.OpenReferralService.RegularSchedules.Serialize())
                UpdateRegularSchedule(entity.Regular_schedules, request.OpenReferralService.RegularSchedules ?? new Collection<OpenReferralRegularScheduleDto>(), null);
            if (request.OpenReferralService.HolidaySchedules != null && entity.Holiday_schedules.Serialize() != request.OpenReferralService.HolidaySchedules.Serialize())
                UpdateHolidaySchedule(entity.Holiday_schedules, request.OpenReferralService.HolidaySchedules ?? new Collection<OpenReferralHolidayScheduleDto>(), null);

            await _context.SaveChangesAsync(cancellationToken);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }

    private void UpdateEligibility(ICollection<OpenReferralEligibility> existing, ICollection<OpenReferralEligibilityDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedEligibility in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedEligibility.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralEligibility>(updatedEligibility);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralEligibilityCreatedEvent(entity));
                _context.OpenReferralEligibilities.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                currentIds.Add(updatedEligibility.Id);
                current.Eligibility = updatedEligibility.Eligibility;
                current.Maximum_age = updatedEligibility.Maximum_age;
                current.Minimum_age = updatedEligibility.Minimum_age;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralEligibilities.RemoveRange(dataToDelete);
    }

    private void UpdateServiceAtLocation(ICollection<OpenReferralServiceAtLocation> existing, ICollection<OpenReferralServiceAtLocationDto> updated)
    {
        List<string> list = new();
        List<string> listAddress = new();

        foreach (var updatedServiceLoc in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceLoc.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralServiceAtLocation>(updatedServiceLoc);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralServiceAtLocationCreatedEvent(entity));
                _context.OpenReferralServiceAtLocations.Add(entity);
                list.Add(entity.Id);
                if (entity.Location.Physical_addresses != null)
                {
                    foreach (var address in entity.Location.Physical_addresses)
                    {
                        address.OpenReferralLocationId = entity.Location.Id;
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

                if (updatedServiceLoc.Location.Physical_addresses != null)
                {
                    foreach (var address in updatedServiceLoc.Location.Physical_addresses)
                    {
                        var currentAddress = _context.OpenReferralPhysical_Addresses.FirstOrDefault(x => x.Id == address.Id);
                        if (currentAddress == null)
                        {
                            var entity = _mapper.Map<OpenReferralPhysical_Address>(address);
                            entity.OpenReferralLocationId = updatedServiceLoc.Location.Id;
                            entity.RegisterDomainEvent(new OpenReferralPhysicalAddressCreatedEvent(entity));
                            _context.OpenReferralPhysical_Addresses.Add(entity);
                            listAddress.Add(entity.Id);
                        }
                        else
                        {
                            currentAddress.OpenReferralLocationId = updatedServiceLoc.Location.Id;
                            currentAddress.Address_1 = address.Address_1;
                            currentAddress.City = address.City;
                            currentAddress.Postal_code = address.Postal_code;
                            currentAddress.Country = address.Country;
                            currentAddress.State_province = address.State_province;
                            listAddress.Add(currentAddress.Id);
                        }
                    }
                    if (updatedServiceLoc.Regular_schedule != null && current.Regular_schedule != null && current.Regular_schedule.Serialize() != updatedServiceLoc.Regular_schedule.Serialize())
                    {
                        UpdateRegularSchedule(current.Regular_schedule ?? new Collection<OpenReferralRegular_Schedule>(), updatedServiceLoc.Regular_schedule ?? new Collection<OpenReferralRegularScheduleDto>(), current);
                    }
                    if (current.HolidayScheduleCollection != null && updatedServiceLoc.HolidayScheduleCollection != null && current.HolidayScheduleCollection.Serialize() != updatedServiceLoc.HolidayScheduleCollection.Serialize())
                    {
                        UpdateHolidaySchedule(current.HolidayScheduleCollection ?? new Collection<OpenReferralHoliday_Schedule>(), updatedServiceLoc.HolidayScheduleCollection ?? new Collection<OpenReferralHolidayScheduleDto>(), current);
                    }
                }
                if (updatedServiceLoc.Location.LinkTaxonomies != null && updatedServiceLoc.Location.LinkTaxonomies.Any())
                {
                    foreach (var linkTaxonomyDto in updatedServiceLoc.Location.LinkTaxonomies)
                    {
                        var linkTaxonomy =  _context.OpenReferralLinkTaxonomies.SingleOrDefault(p => p.Id == linkTaxonomyDto.Id);
                        if (linkTaxonomy != null)
                        {
                            linkTaxonomy.LinkType = linkTaxonomyDto.LinkType;
                            linkTaxonomy.LinkId = linkTaxonomyDto.LinkId;

                            if (linkTaxonomyDto.Taxonomy != null)
                            {
                                var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => linkTaxonomy.Taxonomy != null && x.Id == linkTaxonomy.Taxonomy.Id);
                                if (taxonomy != null)
                                {
                                    linkTaxonomy.Taxonomy = taxonomy;
                                }
                            }
                        }
                        else
                        {
                            var linkTaxonomyEntity = _mapper.Map<OpenReferralLinkTaxonomy>(linkTaxonomyDto);

                            if (linkTaxonomyEntity.Taxonomy != null)
                            {
                                var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == linkTaxonomyEntity.Taxonomy.Id);
                                if (taxonomy != null)
                                {
                                    linkTaxonomyEntity.Taxonomy = taxonomy;
                                }
                            }

                            ArgumentNullException.ThrowIfNull(linkTaxonomyEntity, nameof(linkTaxonomyEntity));

                            linkTaxonomyEntity.RegisterDomainEvent(new OpenReferralLinkTaxonomyCreatedEvent(linkTaxonomyEntity));

                             _context.OpenReferralLinkTaxonomies.Add(linkTaxonomyEntity);
                        }
                    }
                }

                list.Add(current.Id);
            }
        }

        foreach (var item in existing)
        {
            if (item.Location.Physical_addresses != null)
            {
                foreach (var address in item.Location.Physical_addresses)
                {
                    if (!listAddress.Contains(address.Id))
                    {
                        _context.OpenReferralPhysical_Addresses.Remove(address);
                    }
                }
            }
            

            if (!list.Contains(item.Id))
            {
                _context.OpenReferralServiceAtLocations.Remove(item);
            }
        }
        
    }

    private void UpdateHolidaySchedule(ICollection<OpenReferralHoliday_Schedule> existing, ICollection<OpenReferralHolidayScheduleDto> updated, OpenReferralServiceAtLocation? serviceAtLocation)
    {
        List<string> currentIds = new();
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralHoliday_Schedule>(updatedSchedule);
                if (serviceAtLocation != null)
                    entity.OpenReferralServiceAtLocationId = serviceAtLocation.Id;
                entity.RegisterDomainEvent(new OpenReferralHolidayScheduleCreatedEvent(entity));
                _context.OpenReferralHoliday_Schedules.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Closed = updatedSchedule.Closed;
                current.Closes_at = updatedSchedule.Closes_at;
                current.Start_date = updatedSchedule.Start_date;
                current.End_date = updatedSchedule.End_date;
                current.Opens_at = updatedSchedule.Opens_at;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralHoliday_Schedules.RemoveRange(dataToDelete);
    }

    private void UpdateRegularSchedule(ICollection<OpenReferralRegular_Schedule> existing, ICollection<OpenReferralRegularScheduleDto> updated, OpenReferralServiceAtLocation? serviceAtLocation)
    {
        List<string> currentIds = new();
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralRegular_Schedule>(updatedSchedule);
                if (serviceAtLocation != null)
                    entity.OpenReferralServiceAtLocationId = serviceAtLocation.Id;
                entity.RegisterDomainEvent(new OpenReferralRegularScheduleCreatedEvent(entity));
                _context.OpenReferralRegular_Schedules.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Description = updatedSchedule.Description;
                current.Opens_at = updatedSchedule.Opens_at;
                current.Closes_at = updatedSchedule.Closes_at;
                current.Byday = updatedSchedule.Byday;
                current.Bymonthday = updatedSchedule.Bymonthday;
                current.Dtstart = updatedSchedule.Dtstart;
                current.Freq = updatedSchedule.Freq;
                current.Interval = updatedSchedule.Interval;
                current.Valid_from = updatedSchedule.Valid_from;
                current.Valid_to = updatedSchedule.Valid_to;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralRegular_Schedules.RemoveRange(dataToDelete);
    }

    private void UpdateServiceArea(ICollection<OpenReferralService_Area> existing, ICollection<OpenReferralServiceAreaDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedServiceArea in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceArea.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralService_Area>(updatedServiceArea);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralServiceAreaCreatedEvent(entity));
                _context.OpenReferralService_Areas.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Service_area = updatedServiceArea.Service_area;
                current.Extent = updatedServiceArea.Extent;
                current.Uri = updatedServiceArea.Uri;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralService_Areas.RemoveRange(dataToDelete);
    }

    private void UpdateCostOptions(ICollection<OpenReferralCost_Option> existing, ICollection<OpenReferralCostOptionDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedCostOption in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedCostOption.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralCost_Option>(updatedCostOption);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralCostOptionCreatedEvent(entity));
                _context.OpenReferralCost_Options.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.LinkId = updatedCostOption.LinkId;
                current.Amount = updatedCostOption.Amount;
                current.Amount_description = updatedCostOption.Amount_description;
                current.Option = updatedCostOption.Option;
                current.Valid_from = updatedCostOption.Valid_from;
                current.Valid_to = updatedCostOption.Valid_to;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralCost_Options.RemoveRange(dataToDelete);

    }

    private void UpdateTaxonomies(ICollection<OpenReferralService_Taxonomy> existing, ICollection<OpenReferralServiceTaxonomyDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedServiceTaxonomy in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralService_Taxonomy>(updatedServiceTaxonomy);
                if (updatedServiceTaxonomy.Taxonomy != null)
                    entity.Taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Taxonomy.Id);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralServiceTaxonomyCreatedEvent(entity));
                _context.OpenReferralService_Taxonomies.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                if (current.Taxonomy != null && updatedServiceTaxonomy.Taxonomy != null)
                {
                    current.Taxonomy.Name = updatedServiceTaxonomy.Taxonomy.Name;
                    current.Taxonomy.Vocabulary = updatedServiceTaxonomy.Taxonomy.Vocabulary;
                    current.Taxonomy.Parent = updatedServiceTaxonomy.Taxonomy.Parent;
                }

                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralService_Taxonomies.RemoveRange(dataToDelete);
    }

    private void UpdateLanguages(ICollection<OpenReferralLanguage> existing, ICollection<OpenReferralLanguageDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedLanguage in updated)
        {
            var current = existing.FirstOrDefault(x => x.Language == updatedLanguage.Language && x.OpenReferralServiceId == _request.OpenReferralService.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralLanguage>(updatedLanguage);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralLanguageCreatedEvent(entity));
                _context.OpenReferralLanguages.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.Language = updatedLanguage.Language;
                currentIds.Add(current.Id);
            }
        }
        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralLanguages.RemoveRange(dataToDelete);
    }

    

    private void UpdateContacts(ICollection<OpenReferralContact> existing, ICollection<OpenReferralContactDto> updated)
    {
        foreach (var updatedContact in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedContact.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralContact>(updatedContact);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralContactCreatedEvent(entity));
                _context.OpenReferralContacts.Add(entity);
            }
            else
            {
                current.Title = updatedContact.Title;
                current.Name = updatedContact.Name;
                current.Telephone = updatedContact.Telephone;
                current.TextPhone = updatedContact.TextPhone;
            }
        }

        var contactToDelete = existing.Where(a => !existing.Select(x => x.Id).Contains(a.Id)).ToList();
        if (contactToDelete.Any())
        {
            _context.OpenReferralContacts.RemoveRange(contactToDelete);
        }
            
    }

    private void UpdateServiceDelivery(ICollection<OpenReferralServiceDelivery> existing, ICollection<OpenReferralServiceDeliveryExDto> updated)
    {
        List<string> currentIds = new();
        foreach (var updatedServiceDelivery in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralServiceDelivery>(updatedServiceDelivery);
                entity.OpenReferralServiceId = _request.OpenReferralService.Id;
                entity.RegisterDomainEvent(new OpenReferralServiceDeliveryCreatedEvent(entity));
                _context.OpenReferralServiceDeliveries.Add(entity);
                currentIds.Add(entity.Id);
            }
            else
            {
                current.ServiceDelivery = updatedServiceDelivery.ServiceDelivery;
                currentIds.Add(current.Id);
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralServiceDeliveries.RemoveRange(dataToDelete);
    }
}


