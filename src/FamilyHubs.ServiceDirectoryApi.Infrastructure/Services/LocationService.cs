using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services
{
    public interface ILocationService
    {
        Task<List<Location>> GetLocations(LocationQuery queryValues);
    }

    public class LocationService : BaseRepositoryService, ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetLocations(LocationQuery queryValues)
        {
            var locations = _context.Locations.AsQueryable();

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
            locations = locations.Include(x => x.LinkContacts);
            locations = locations.Include(x => x.LinkTaxonomies);

            return await locations.ToListAsync();
        }

    }
}
