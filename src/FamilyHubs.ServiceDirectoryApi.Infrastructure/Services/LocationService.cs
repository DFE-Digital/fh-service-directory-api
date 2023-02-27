using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services
{
    public interface ILocationService
    {
        List<Location> GetLocations(Dictionary<string, object> QueryValues);
    }

    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Location> GetLocations(Dictionary<string, object> queryValues)
        {
            var locations = _context.Locations.AsQueryable();
            if (ShouldQuery(queryValues, "Id", out var id))
            {
                locations = locations.Where(x => x.Id == id);
            }
            if (ShouldQuery(queryValues, "Name", out var name))
            {
                locations = locations.Where(x => x.Name == name);
            }
            if (ShouldQuery(queryValues, "Description", out var description))
            {
                locations = locations.Where(x => !string.IsNullOrEmpty(x.Description) && x.Description.Contains(description));
            }

            locations = locations.Include(x => x.PhysicalAddresses);

            return locations.ToList();
        }

        public static bool ShouldQuery(Dictionary<string, object> queryValues, string key, out string queryValue)
        {

            if (!queryValues.ContainsKey(key))
            {
                queryValue = string.Empty;
                return false;
            }

            queryValue = (string)queryValues[key];//Has to be string because of overloads

            if (string.IsNullOrEmpty(queryValue))
            {
                return false;
            }

            return true;
        }

    }
}
