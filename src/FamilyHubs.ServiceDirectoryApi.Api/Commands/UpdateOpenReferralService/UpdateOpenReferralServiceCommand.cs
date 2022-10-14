using Ardalis.GuardClauses;
using Ardalis.Specification;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralHolidaySchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhones;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

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

    public UpdateOpenReferralServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateOpenReferralServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.OpenReferralServices
           .Include(x => x.ServiceDelivery)
           .Include(x => x.Eligibilities)
           .Include(x => x.Contacts)
           .ThenInclude(x => x.Phones)
           .Include(x => x.Cost_options)
           .Include(x => x.Languages)
           .Include(x => x.Service_areas)
           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.Physical_addresses)
           .Include(x => x.Service_taxonomys)
           .ThenInclude(x => x.Taxonomy)
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        try
        {
            var serviceentity = _mapper.Map<OpenReferralService>(request.OpenReferralService);
            ArgumentNullException.ThrowIfNull(serviceentity, nameof(serviceentity));

            entity.Name = serviceentity.Name;
            entity.Description = serviceentity.Description;
            entity.Accreditations = serviceentity.Accreditations;
            entity.Assured_date = serviceentity.Assured_date;
            entity.Attending_access = serviceentity.Attending_access;
            entity.Attending_type = serviceentity.Attending_type;
            entity.Deliverable_type = serviceentity.Deliverable_type;
            entity.Status = serviceentity.Status;
            entity.Url = serviceentity.Url;
            entity.Email = serviceentity.Email;
            entity.Fees = serviceentity.Fees;

            if (entity.Eligibilities.Serialize() != request?.OpenReferralService?.Eligibilities?.Serialize())
                UpdateEligibility(entity.Eligibilities, request?.OpenReferralService.Eligibilities ?? new Collection<OpenReferralEligibilityDto>());
            if (entity.Service_areas.Serialize() != request?.OpenReferralService.Service_areas?.Serialize())
                UpdateServiceArea(entity.Service_areas, request?.OpenReferralService.Service_areas ?? new Collection<OpenReferralServiceAreaDto>());
            if (entity.ServiceDelivery.Serialize() != request?.OpenReferralService.ServiceDelivery?.Serialize())
                UpdateServiceDelivery(entity.ServiceDelivery, request?.OpenReferralService.ServiceDelivery ?? new Collection<OpenReferralServiceDeliveryExDto>());
            if (entity.Contacts.Serialize() != request?.OpenReferralService?.Contacts?.Serialize())
                UpdateContacts(entity.Contacts, request?.OpenReferralService.Contacts ?? new Collection<OpenReferralContactDto>());
            if (entity.Languages.Serialize() != request?.OpenReferralService?.Languages?.Serialize())
                UpdateLanguages(entity.Languages, request?.OpenReferralService.Languages ?? new Collection<OpenReferralLanguageDto>());
            if (entity.Service_at_locations.Serialize() != request?.OpenReferralService?.Service_at_locations?.Serialize())
                UpdateServiceAtLocation(entity.Service_at_locations, request?.OpenReferralService.Service_at_locations ?? new Collection<OpenReferralServiceAtLocationDto>());
            if (entity.Service_taxonomys?.Serialize() != request?.OpenReferralService?.Service_taxonomys?.Serialize())
                UpdateTaxonomies(entity.Service_taxonomys ?? new Collection<OpenReferralService_Taxonomy>(), request?.OpenReferralService?.Service_taxonomys ?? new Collection<OpenReferralServiceTaxonomyDto>());
            if (entity.Cost_options?.Serialize() != request?.OpenReferralService?.Cost_options?.Serialize())
                UpdateCostOptions(entity.Cost_options ?? new Collection<OpenReferralCost_Option>(), request?.OpenReferralService?.Cost_options ?? new Collection<OpenReferralCostOptionDto>());

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
        foreach (var updatedEligibility in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedEligibility.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralEligibility>(updatedEligibility);
                entity.RegisterDomainEvent(new OpenReferralEligibilityEvent(entity));
                _context.OpenReferralEligibilities.Add(entity);
            }
            else
            {
                //current.LinkId = updatedEligibility.LinkId;
                current.Eligibility = updatedEligibility.Eligibility;
                current.Maximum_age = updatedEligibility.Maximum_age;
                current.Minimum_age = updatedEligibility.Minimum_age;
                //current.Taxonomys = updatedEligibility.Taxonomys;
            }
        }
    }

    private void UpdateServiceAtLocation(ICollection<OpenReferralServiceAtLocation> existing, ICollection<OpenReferralServiceAtLocationDto> updated)
    {
        foreach (var updatedServiceLoc in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceLoc.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralServiceAtLocation>(updatedServiceLoc);
                entity.RegisterDomainEvent(new OpenReferralServiceAtLocationEvent(entity));
                _context.OpenReferralServiceAtLocations.Add(entity);
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
                            entity.RegisterDomainEvent(new OpenReferralPhysicalAddressEvent(entity));
                            _context.OpenReferralPhysical_Addresses.Add(entity);
                        }
                        else
                        {
                            currentAddress.Address_1 = address.Address_1;
                            currentAddress.City = address.City;
                            currentAddress.Postal_code = address.Postal_code;
                            currentAddress.Country = address.Country;
                            currentAddress.State_province = address.State_province;
                        }
                    }
                    if (current?.Regular_schedule?.Serialize() != updatedServiceLoc?.Regular_schedule?.Serialize())
                    {
                        UpdateRegularSchedule(current?.Regular_schedule ?? new Collection<OpenReferralRegular_Schedule>(), updatedServiceLoc?.Regular_schedule ?? new Collection<OpenReferralRegularScheduleDto>());
                    }
                    if (current?.HolidayScheduleCollection?.Serialize() != updatedServiceLoc?.HolidayScheduleCollection?.Serialize())
                    {
                        UpdateHolidaySchedule(current?.HolidayScheduleCollection ?? new Collection<OpenReferralHoliday_Schedule>(), updatedServiceLoc?.HolidayScheduleCollection ?? new Collection<OpenReferralHolidayScheduleDto>());
                    }
                    
                }
            }
        }
    }

    private void UpdateHolidaySchedule(ICollection<OpenReferralHoliday_Schedule> existing, ICollection<OpenReferralHolidayScheduleDto> updated)
    {
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralHoliday_Schedule>(updatedSchedule);
                entity.RegisterDomainEvent(new OpenReferralHolidayScheduleEvent(entity));
                _context.OpenReferralHoliday_Schedules.Add(entity);
            }
            else
            {
                current.Closed = updatedSchedule.Closed;
                current.Closes_at = updatedSchedule.Closes_at;
                current.Start_date = updatedSchedule.Start_date;
                current.End_date = updatedSchedule.End_date;
                current.Opens_at = updatedSchedule.Opens_at;
            }
        }
    }

    private void UpdateRegularSchedule(ICollection<OpenReferralRegular_Schedule> existing, ICollection<OpenReferralRegularScheduleDto> updated)
    {
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralRegular_Schedule>(updatedSchedule);
                entity.RegisterDomainEvent(new OpenReferralRegularScheduleEvent(entity));
                _context.OpenReferralRegular_Schedules.Add(entity);
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
            }
        }
    }

    private void UpdateServiceArea(ICollection<OpenReferralService_Area> existing, ICollection<OpenReferralServiceAreaDto> updated)
    {
        foreach (var updatedServiceArea in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceArea.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralService_Area>(updatedServiceArea);
                entity.RegisterDomainEvent(new OpenReferralServiceAreaEvent(entity));
                _context.OpenReferralService_Areas.Add(entity);
            }
            else
            {
                //current.LinkId = updatedServiceArea.LinkId;
                current.Service_area = updatedServiceArea.Service_area;
                current.Extent = updatedServiceArea.Extent;
                current.Uri = updatedServiceArea.Uri;
            }
        }
    }

    private void UpdateCostOptions(ICollection<OpenReferralCost_Option> existing, ICollection<OpenReferralCostOptionDto> updated)
    {
        foreach (var updatedCostOption in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedCostOption.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralCost_Option>(updatedCostOption);
                entity.RegisterDomainEvent(new OpenReferralCostOptionEvent(entity));
                _context.OpenReferralCost_Options.Add(entity);
            }
            else
            {
                current.LinkId = updatedCostOption.LinkId;
                current.Amount = updatedCostOption.Amount;
                current.Amount_description = updatedCostOption.Amount_description;
                current.Option = updatedCostOption.Option;
                current.Valid_from = updatedCostOption.Valid_from;
                current.Valid_to = updatedCostOption.Valid_to;
            }
        }
    }

    private void UpdateTaxonomies(ICollection<OpenReferralService_Taxonomy> existing, ICollection<OpenReferralServiceTaxonomyDto> updated)
    {
        foreach (var updatedServiceTaxonomy in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Id);
            if (current == null)
            {
                if (updatedServiceTaxonomy.Taxonomy != null)
                {
                    var currentTaxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x =>x.Id == updatedServiceTaxonomy.Taxonomy.Id);
                    if (currentTaxonomy == null)
                    {
                        var entityTaxonomy = _mapper.Map<OpenReferralTaxonomy>(updatedServiceTaxonomy.Taxonomy);
                        entityTaxonomy.RegisterDomainEvent(new OpenReferralTaxonomyCreatedEvent(entityTaxonomy));
                        _context.OpenReferralTaxonomies.Add(entityTaxonomy);
                    }
                    else
                    {
                        currentTaxonomy.Name = updatedServiceTaxonomy.Taxonomy.Name;
                        currentTaxonomy.Vocabulary = updatedServiceTaxonomy.Taxonomy.Vocabulary;
                        currentTaxonomy.Parent = updatedServiceTaxonomy.Taxonomy.Parent;
                    }
                }

                var entity = _mapper.Map<OpenReferralService_Taxonomy>(updatedServiceTaxonomy);
                entity.RegisterDomainEvent(new OpenReferralServiceTaxonomyEvent(entity));
                _context.OpenReferralService_Taxonomies.Add(entity);
            }
            else
            {
                if (current.Taxonomy != null && updatedServiceTaxonomy.Taxonomy != null)
                {
                    current.Taxonomy.Name = updatedServiceTaxonomy.Taxonomy.Name;
                    current.Taxonomy.Vocabulary = updatedServiceTaxonomy.Taxonomy.Vocabulary;
                    current.Taxonomy.Parent = updatedServiceTaxonomy.Taxonomy.Parent;
                } 
            }
        }
    }

    private void UpdateLanguages(ICollection<OpenReferralLanguage> existing, ICollection<OpenReferralLanguageDto> updated)
    {
        foreach (var updatedLanguage in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedLanguage.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralLanguage>(updatedLanguage);
                entity.RegisterDomainEvent(new OpenReferralLanguageEvent(entity));
                _context.OpenReferralLanguages.Add(entity);
            }
            else
            {
                current.Language = updatedLanguage.Language;
            }
        }
    }

    

    private void UpdateContacts(ICollection<OpenReferralContact> existing, ICollection<OpenReferralContactDto> updated)
    {
        foreach (var updatedContact in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedContact.Id);
            if (current == null)
            {
                List<OpenReferralPhone> listPhones = new();
                if (updatedContact.Phones != null && updatedContact.Phones.Any())
                {
                    foreach (var phone in updatedContact.Phones)
                    {
                        var existingphone = _context.OpenReferralPhones.FirstOrDefault(x => x.Id == phone.Id);
                        if (existingphone == null)
                        {
                            var phoneentity = _mapper.Map<OpenReferralPhone>(phone);
                            phoneentity.RegisterDomainEvent(new OpenReferralPhoneEvent(phoneentity));
                            _context.OpenReferralPhones.Add(phoneentity);
                            listPhones.Add(phoneentity);
                        }
                        else
                        {
                            existingphone.Number = phone.Number;
                        }

                    }
                }
                
                var entity = _mapper.Map<OpenReferralContact>(updatedContact);
                entity.Phones = listPhones;
                entity.RegisterDomainEvent(new OpenReferralContactEvent(entity));
                _context.OpenReferralContacts.Add(entity);
            }
            else
            {
                current.Title = updatedContact.Title;
                current.Name = updatedContact.Name;
                if (updatedContact.Phones != null)
                {
                    foreach (var phone in updatedContact.Phones)
                    {
                        var existingphone = _context.OpenReferralPhones.FirstOrDefault(x => x.Id == phone.Id);
                        if (existingphone == null)
                        {
                            var entity = _mapper.Map<OpenReferralPhone>(phone);
                            entity.RegisterDomainEvent(new OpenReferralPhoneEvent(entity));
                            _context.OpenReferralPhones.Add(entity);
                        }
                        else
                        {
                            existingphone.Number = phone.Number;
                        }
                    }
                }
            }
        }
    }

    private void UpdateServiceDelivery(ICollection<OpenReferralServiceDelivery> existing, ICollection<OpenReferralServiceDeliveryExDto> updated)
    {
        foreach (var updatedServiceDelivery in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
            if (current == null)
            {
                var entity = _mapper.Map<OpenReferralServiceDelivery>(updatedServiceDelivery);
                entity.RegisterDomainEvent(new OpenReferralServiceDeliveryEvent(entity));
                _context.OpenReferralServiceDeliveries.Add(entity);
            }
            else
            {
                current.ServiceDelivery = updatedServiceDelivery.ServiceDelivery;
            }
        }
    }
}


