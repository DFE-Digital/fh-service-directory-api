using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using IdGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services
{
    public interface ILocationService
    {
        Task<List<Location>> GetLocations(LocationQuery queryValues);
        Task<Location> Upsert(Location location);
        Task<Location?> GetById(string id);
        Task<Location?> GetLocation(Location location);
    }

    public class LocationService : BaseRepositoryService<Location, LocationService>, ILocationService
    {

        public LocationService(ILogger<LocationService> logger, IIdGenerator<long> idGenerator, ApplicationDbContext context) 
            : base(logger, idGenerator, context, context.Locations) 
        {

        }

        public override async Task<Location?> GetById(string id)
        {
            return await DbContext.Locations.Where(x => x.Id == id)
                .Include(x => x.LinkTaxonomies!).ThenInclude(x => x.Taxonomy)
                .Include(x => x.PhysicalAddresses)
                .Include(x => x.AccessibilityForDisabilities)
                .Include(x => x.LinkContacts!).ThenInclude(x => x.Contact)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Location>> GetLocations(LocationQuery queryValues)
        {
            var locations = DbContext.Locations.AsQueryable();

            if (!string.IsNullOrEmpty(queryValues.Id))
                locations = locations.Where(x => x.Id == queryValues.Id);

            if (!string.IsNullOrEmpty(queryValues.Name))
                locations = locations.Where(x => x.Name == queryValues.Name);

            if (!string.IsNullOrEmpty(queryValues.Description))
                locations = locations.Where(x => !string.IsNullOrEmpty(x.Description) && x.Description.Contains(queryValues.Description));

            if (!string.IsNullOrEmpty(queryValues.PostCode))
                locations = locations.Where(x => 
                    x.PhysicalAddresses != null && 
                    x.PhysicalAddresses.Where(y=>y.PostCode.ToLower().Replace(" ", "") == queryValues.PostCode.ToLower().Replace(" ", "")).Any());

            locations = locations.Include(x => x.PhysicalAddresses);
            locations = locations.Include(x => x.AccessibilityForDisabilities);
            locations = locations.Include(x => x.LinkContacts!).ThenInclude(x => x.Contact);
            locations = locations.Include(x => x.LinkTaxonomies!).ThenInclude(x => x.Taxonomy);

            return await locations.ToListAsync();
        }

        public async Task<Location?> GetLocation(Location location)
        {
            if (!string.IsNullOrEmpty(location.Id))
            {
                return await GetById(location.Id);
            }

            var query = new LocationQuery { Name = location.Name, PostCode = location.PhysicalAddresses?.First().PostCode };
            return (await GetLocations(query)).FirstOrDefault(); 
        }

        protected override void UpdateEntityValues(Location existing, Location modified)
        {
            existing.Name = modified.Name;
            existing.Description = modified.Description;
            existing.Latitude = modified.Latitude;
            existing.Longitude = modified.Longitude;

            if(existing.LinkContacts != null)
            {
                foreach (var link in existing.LinkContacts)
                {
                    DbContext.Entry(link.Contact!).State = EntityState.Unchanged;
                }
            }

        }
    }
}
