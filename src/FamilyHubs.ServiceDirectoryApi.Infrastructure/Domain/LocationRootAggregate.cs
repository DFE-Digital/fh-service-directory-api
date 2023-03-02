using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Domain
{
    public interface ILocationRootAggregate
    {
        Task<Location> Upsert(Location dto);
    }

    public class LocationRootAggregate : ILocationRootAggregate
    {
        private readonly IContactService _contactService;
        private readonly ILocationService _locationService;

        public LocationRootAggregate(ILocationService locationService, IContactService contactService)
        {
            _locationService = locationService;
            _contactService = contactService;
        }

        public async Task<Location> Upsert(Location locationWithNewValues)
        {
            var locationWithoutChildObjects = GetLocationWithoutChildObjects(locationWithNewValues);
            var existingLocationRecord = await _locationService.Upsert(locationWithoutChildObjects);
            var locationId = existingLocationRecord.Id;

            //Handle Child objects
            if (locationWithNewValues.LinkContacts is not null)
            {
                foreach (var linkContact in locationWithNewValues.LinkContacts)
                {
                    await _contactService.HydrateLinkContact(linkContact, locationId, nameof(Location));
                }
            }
            existingLocationRecord.LinkContacts = locationWithNewValues.LinkContacts;

            existingLocationRecord = await _locationService.Upsert(existingLocationRecord);
            return existingLocationRecord;
        }

        private Location GetLocationWithoutChildObjects(Location location)
        {
            return new Location(location.Id, location.Name, location.Description, location.Latitude, location.Longitude, null, null, null, new List<LinkContact>());
        }
    }
}
