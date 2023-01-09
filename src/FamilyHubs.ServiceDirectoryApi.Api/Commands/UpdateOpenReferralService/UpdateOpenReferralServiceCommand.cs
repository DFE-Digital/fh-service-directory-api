using Ardalis.GuardClauses;
using Ardalis.Specification;
using AutoMapper;
using Azure.Core;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralHolidaySchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLinkContacts;
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
using System.Collections.ObjectModel;

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

    public UpdateOpenReferralServiceCommandHandler(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<UpdateOpenReferralServiceCommandHandler> logger
    )
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        OpenReferralService? efOpenReferralService = await GetEfOpenReferralService(request, cancellationToken);

        try
        {
            await UpdateEfOpenReferralService(request, efOpenReferralService, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating Service. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return efOpenReferralService.Id;
    }

    private async Task UpdateEfOpenReferralService(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity, CancellationToken cancellationToken)
    {
        var serviceEntity = _mapper.Map<OpenReferralService>(request.OpenReferralService);
        ArgumentNullException.ThrowIfNull(serviceEntity, nameof(serviceEntity));

        MapServiceEntityToDbServiceEntity(request, serviceEntity, dbServiceEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void MapServiceEntityToDbServiceEntity(
        UpdateOpenReferralServiceCommand request,
        OpenReferralService serviceEntity,
        OpenReferralService dbServiceEntity
    )
    {
        UpdateServiceType(request, dbServiceEntity);
        UpdateService(serviceEntity, dbServiceEntity);
        UpdateLanguages(request, serviceEntity, dbServiceEntity);
        UpdateServiceTaxonomies(request, serviceEntity, dbServiceEntity);

        // TODO: UpdateFunding(request, serviceEntity, dbServiceEntity);
        UpdateCostOptions(request, serviceEntity, dbServiceEntity);
        UpdateServiceArea(request, serviceEntity, dbServiceEntity);
        UpdateEligibility(request, serviceEntity, dbServiceEntity);
        UpdateServiceDelivery(request, serviceEntity, dbServiceEntity);

        UpdateContacts(request, dbServiceEntity);
        UpdateRegularSchedule(request, entity.Regular_schedules ?? new Collection<OpenReferralRegular_Schedule>(), request?.OpenReferralService.RegularSchedules ?? new Collection<OpenReferralRegularScheduleDto>(), null);
        //UpdateRegularSchedule(request, serviceEntity, dbServiceEntity);
        UpdateHolidaySchedule(request, serviceEntity, dbServiceEntity);
        UpdateServiceAtLocation(request, serviceEntity, dbServiceEntity);
        // TODO: UpdateLinkContacts(request, serviceEntity, dbServiceEntity);

        // GG: For the 'standardised spreadsheet' mechanism of importing services we are collecting Service, Location (address) and Contact data
        //     On that basis if the ServiceName is blank or the Location data is blank or the Contact data is blank that is considered to be an error and should have been caught before calling this API.
        //     There is a 1 to Many relationship between Services and Locations, i.e. a local authority may have a Service delivered at multiple locations, such as an antenatal service at a Family Hub
        //     In this instance, there would be 1 row in the Service table for 'Antenatal' and multiple rows in the Location table for all of the Locations (Family Hubs or other) where the Antenatal service is delivered.
        //     For each combination of Service at a given Location will be the Contact details for that Service at the given location.
        //     The ORDM does not support Contact data in the 'service_at_location' table so we have extended the ORDM to allow Contact information to be stored against the 'service_at_location' by creating a link_contact table
        //     The link_contact table follows the existing convention of the link_taxonomy table to allow the 'contact' table to be linked to multiple tables, in this case, the 'service_at_location' but contact data can also be associated with the 'organisation', 'service' and 'location' table.
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
    }

    private async Task<OpenReferralService> GetEfOpenReferralService(UpdateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        var dbServiceEntity = await _context.OpenReferralServices
           .Include(x => x.ServiceType)
           .Include(x => x.ServiceDelivery)
           .Include(x => x.Eligibilities)
           .Include(x => x.Link_Contacts)
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

        if (dbServiceEntity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        return dbServiceEntity;
    }

    private void UpdateServiceType(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (request.OpenReferralService != null && request.OpenReferralService.ServiceType != null)
        {
            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.OpenReferralService.ServiceType.Id);
            if (serviceType != null)
                dbServiceEntity.ServiceType = serviceType;
        }
    }
    
    private static void UpdateService(OpenReferralService serviceEntity, OpenReferralService dbServiceEntity)
    {
        dbServiceEntity.Name = serviceEntity.Name;
        dbServiceEntity.Description = serviceEntity.Description;
        dbServiceEntity.Accreditations = serviceEntity.Accreditations;
        dbServiceEntity.Assured_date = serviceEntity.Assured_date;
        dbServiceEntity.Attending_access = serviceEntity.Attending_access;
        dbServiceEntity.Attending_type = serviceEntity.Attending_type;
        dbServiceEntity.Deliverable_type = serviceEntity.Deliverable_type;
        dbServiceEntity.Status = serviceEntity.Status;
        dbServiceEntity.Url = serviceEntity.Url;
        dbServiceEntity.Email = serviceEntity.Email;
        dbServiceEntity.Fees = serviceEntity.Fees;
    }

    private void UpdateContacts(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.ServiceDelivery.Serialize() != request?.OpenReferralService.ServiceDelivery?.Serialize())
        {
            var updatedContacts = request?.OpenReferralService.Contacts ?? new Collection<OpenReferralContactDto>();
            var existingContacts = dbServiceEntity.Contacts;
            List<string> contactIds = new();
            List<string> phoneIds = new();

            foreach (var updatedContact in updatedContacts)
            {
                var currentContact = existingContacts.FirstOrDefault(x => x.Id == updatedContact.Id);
                if (currentContact == null)
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
                                phoneentity.OpenReferralContactId = updatedContact.Id;
                                phoneentity.RegisterDomainEvent(new OpenReferralPhoneCreatedEvent(phoneentity));
                                _context.OpenReferralPhones.Add(phoneentity);
                                listPhones.Add(phoneentity);
                                phoneIds.Add(phoneentity.Id);
                            }
                            else
                            {
                                existingphone.Number = phone.Number;
                                phoneIds.Add(existingphone.Id);
                            }

                        }
                    }

                    var entity = _mapper.Map<OpenReferralContact>(updatedContact);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    entity.Phones = listPhones;
                    _context.OpenReferralContacts.Add(entity);
                    contactIds.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralContactCreatedEvent(entity));
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
                                entity.OpenReferralContactId = current.Id;
                                entity.RegisterDomainEvent(new OpenReferralPhoneCreatedEvent(entity));
                                _context.OpenReferralPhones.Add(entity);
                                phoneIds.Add(entity.Id);
                            }
                            else
                            {
                                existingphone.Number = phone.Number;
                                phoneIds.Add(existingphone.Id);
                            }
                        }
                    }
                    contactIds.Add(current.Id);
                }
            }

            foreach (var contact in existingContacts)
            {
                if (contact != null && contact.Phones != null)
                {
                    foreach (var phone in contact.Phones)
                    {
                        if (!phoneIds.Contains(phone.Id))
                        {
                            _context.OpenReferralPhones.Remove(phone);
                        }
                    }
                }
            }

            var contactToDelete = existing.Contacts.Where(a => !existing.Contacts.Select(x => x.Id).Contains(a.Id)).ToList();
            if (contactToDelete != null && contactToDelete.Any())
            {
                _context.OpenReferralContacts.RemoveRange(contactToDelete);
            }
        }
    }

    private void UpdateServiceTaxonomies(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.Service_taxonomys?.Serialize() != request?.OpenReferralService?.Service_taxonomys?.Serialize())
        {
            var updatedServiceTaxonomies = request?.OpenReferralService.Service_taxonomys ?? new Collection<OpenReferralServiceTaxonomyDto>();
            var existingServiceTaxonomies = dbServiceEntity?.Service_taxonomys;

            List<string> currentIds = new();
            foreach (var updatedServiceTaxonomy in updatedServiceTaxonomies)
            {
                var currentServiceTaxonomy = existingServiceTaxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Id);
                if (currentServiceTaxonomy == null)
                {
                    var entity = _mapper.Map<OpenReferralService_Taxonomy>(updatedServiceTaxonomy);
                    if (updatedServiceTaxonomy.Taxonomy != null)
                        entity.Taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Taxonomy.Id);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    _context.OpenReferralService_Taxonomies.Add(entity);
                    currentIds.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralServiceTaxonomyCreatedEvent(entity));
                }
                else
                {
                    if (currentServiceTaxonomy != null && updatedServiceTaxonomy.Taxonomy != null)
                    {
                        currentServiceTaxonomy.Name = updatedServiceTaxonomy.Taxonomy.Name;
                        currentServiceTaxonomy.Vocabulary = updatedServiceTaxonomy.Taxonomy.Vocabulary;
                        currentServiceTaxonomy.Parent = updatedServiceTaxonomy.Taxonomy.Parent;
                    }

                    currentIds.Add(current.Id);
                }
            }

            var dataToDelete = existing.Service_taxonomys.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralService_Taxonomies.RemoveRange(dataToDelete);
        }
    }

    private void UpdateEligibility(UpdateOpenReferralServiceCommand request, OpenReferralService updated, OpenReferralService existing)
    {
        if (updated != null && updated.Eligibilities?.Serialize() != null && existing.Eligibilities.Serialize() != null)
        {
            List<string> currentIds = new();
            foreach (var updatedEligibility in updated.Eligibilities)
            {
                var current = existing.Eligibilities.FirstOrDefault(x => x.Id == updatedEligibility.Id);
                if (current == null)
                {
                    var entity = _mapper.Map<OpenReferralEligibility>(updatedEligibility);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
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
                }
            }

            var dataToDelete = existing.Eligibilities.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralEligibilities.RemoveRange(dataToDelete);
        }
    }

    private void UpdateServiceArea(UpdateOpenReferralServiceCommand request, OpenReferralService updated, OpenReferralService existing)
    {
        if (updated != null && updated.Service_areas?.Serialize() != null && existing.Service_areas.Serialize() != null)
        {
            List<string> currentIds = new();
            foreach (var updatedServiceArea in updated.Service_areas)
            {
                var current = existing.Service_areas.FirstOrDefault(x => x.Id == updatedServiceArea.Id);
                if (current == null)
                {
                    var entity = _mapper.Map<OpenReferralService_Area>(updatedServiceArea);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    entity.RegisterDomainEvent(new OpenReferralServiceAreaCreatedEvent(entity));
                    _context.OpenReferralService_Areas.Add(entity);
                    currentIds.Add(entity.Id);
                }
                else
                {
                    current.Service_area = updatedServiceArea.Service_area;
                    current.Extent = updatedServiceArea.Extent;
                    current.Uri = updatedServiceArea.Uri;
                }
            }

            var dataToDelete = existing.Service_areas.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralService_Areas.RemoveRange(dataToDelete);
        }
    }

    private void UpdateServiceDelivery(UpdateOpenReferralServiceCommand request, OpenReferralService updated, OpenReferralService existing)
    {
        if (updated != null && updated.ServiceDelivery?.Serialize() != null && existing.ServiceDelivery.Serialize() != null)
        {
            List<string> currentIds = new();
            foreach (var updatedServiceDelivery in updated.ServiceDelivery)
            {
                var current = existing.ServiceDelivery.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
                if (current == null)
                {
                    var entity = _mapper.Map<OpenReferralServiceDelivery>(updatedServiceDelivery);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
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

            var dataToDelete = existing.ServiceDelivery.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralServiceDeliveries.RemoveRange(dataToDelete);
        }
    }

    private void UpdateLanguages(UpdateOpenReferralServiceCommand request, OpenReferralService updated, OpenReferralService existing)
    {
        if (updated != null && updated.Languages?.Serialize() != null && existing.Languages.Serialize() != null)
        {
            List<string> currentIds = new();
            foreach (var updatedLanguage in updated.Languages)
            {
                var current = existing.Languages.FirstOrDefault(x => x.Language == updatedLanguage.Language && x.OpenReferralServiceId == request.OpenReferralService.Id);
                if (current == null)
                {
                    var entity = _mapper.Map<OpenReferralLanguage>(updatedLanguage);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
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
            var dataToDelete = existing.Languages.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralLanguages.RemoveRange(dataToDelete);
        }
    }

    private void UpdateServiceAtLocation(UpdateOpenReferralServiceCommand request, OpenReferralService updated, OpenReferralService existing)
    {
        if (updated != null && updated.Service_at_locations?.Serialize() != null && existing.Service_at_locations.Serialize() != null)
        {
            List<string> list = new();
            List<string> listAddress = new();

            foreach (var updatedServiceLoc in updated.Service_at_locations)
            {
                var current = existing.Service_at_locations.FirstOrDefault(x => x.Id == updatedServiceLoc.Id);
                if (current == null)
                {
                    var entity = _mapper.Map<OpenReferralServiceAtLocation>(updatedServiceLoc);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;

                    _context.OpenReferralServiceAtLocations.Add(entity);
                    list.Add(entity.Id);
                    if (entity.Location.Physical_addresses != null)
                    {
                        foreach (var address in entity.Location.Physical_addresses)
                        {
                            listAddress.Add(address.Id);
                        }
                    }
                    entity.RegisterDomainEvent(new OpenReferralServiceAtLocationCreatedEvent(entity));
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
                                entity.RegisterDomainEvent(new OpenReferralPhysicalAddressCreatedEvent(entity));
                                _context.OpenReferralPhysical_Addresses.Add(entity);
                                listAddress.Add(entity.Id);
                            }
                            else
                            {
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
                            UpdateRegularSchedule(request, current.Regular_schedule ?? new Collection<OpenReferralRegular_Schedule>(), updatedServiceLoc.Regular_schedule ?? new Collection<OpenReferralRegularScheduleDto>(), current);
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
                            var linkTaxonomy = _context.OpenReferralLinkTaxonomies.SingleOrDefault(p => p.Id == linkTaxonomyDto.Id);
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

            foreach (var item in existing.Service_at_locations)
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
            }
        }

        var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
        if (dataToDelete.Any())
            _context.OpenReferralHoliday_Schedules.RemoveRange(dataToDelete);
    }

    private void UpdateRegularSchedule(UpdateOpenReferralServiceCommand request, OpenReferralService updated, OpenReferralService existing)
    {
        if (existing.Regular_schedules?.Serialize() != request?.OpenReferralService?.RegularSchedules?.Serialize())
        {

        }

        if (updated != null && updated.Regular_schedules?.Serialize() != null && existing.Regular_schedules.Serialize() != null)
        {
            List<string> currentIds = new();
            foreach (var updatedSchedule in updated.Regular_schedules)
            {
                var current = existing.Regular_schedules.FirstOrDefault(x => x.Id == updatedSchedule.Id);
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
                }
            }

            var dataToDelete = existing.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralRegular_Schedules.RemoveRange(dataToDelete);
        }
    }


    private void UpdateCostOptions(UpdateOpenReferralServiceCommand request, OpenReferralService updated, OpenReferralService existing)
    {
        if (updated != null && updated.Cost_options?.Serialize() != null && existing.Cost_options.Serialize() != null)
        {
            List<string> currentIds = new();
            foreach (var updatedCostOption in updated.Cost_options)
            {
                var current = existing.Cost_options.FirstOrDefault(x => x.Id == updatedCostOption.Id);
                if (current == null)
                {
                    var entity = _mapper.Map<OpenReferralCost_Option>(updatedCostOption);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
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

            var dataToDelete = existing.Cost_options.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralCost_Options.RemoveRange(dataToDelete);
        }
    }


 


 
}


