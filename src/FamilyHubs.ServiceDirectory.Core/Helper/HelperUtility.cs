using System.Diagnostics;
using System.Linq.Expressions;
using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Data.Repository;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Helper;

public static class HelperUtility
{
    //todo: generic for entity
    public static async Task<List<Location>> LinkExistingLocations(this IList<Location> locations, DbSet<Location> existingLocations, IMapper mapper)
    {
        List<Location> linkedLocations = new();
        foreach (var location in locations)
        {
            Location newLoc;
            //or IsKeySet
            if (location.Id != 0)
            {
                var existingLocation = await existingLocations.FindAsync(location.Id)
                    ?? throw new NotFoundException(nameof(Location), location.Id.ToString());

                mapper.Map(location, existingLocation);
                newLoc = existingLocation;
            }
            else
            {
                newLoc = location;
            }

            linkedLocations.Add(newLoc);
        }

        return linkedLocations;
    }

    public static void AttachExistingManyToMany(this Service service, ApplicationDbContext context, IMapper mapper)
    {
        var existingTaxonomies = service.Taxonomies.Select(s => s.Name).ToList();
        service.Taxonomies = service.Taxonomies.AddOrAttachExisting(context, mapper,
            t => existingTaxonomies.Contains(t.Name),
            (s, d) => s.Name == d.Name);
    }

    public static IList<TEntity> AddOrAttachExisting<TEntity>(this IList<TEntity> unSavedEntities,
        ApplicationDbContext context,
        IMapper mapper,
        Expression<Func<TEntity, bool>> searchPredicate,
        Func<TEntity, TEntity, bool> matchingPredicate
    ) where TEntity : EntityBase<long>
    {
        var returnList = new List<TEntity>();

        if (!unSavedEntities.Any())
            return returnList;

        var existing = context.Set<TEntity>().Where(searchPredicate).ToList();

        foreach (var unSavedItem in unSavedEntities)
        {
            var savedItem = existing.SingleOrDefault(saved => matchingPredicate(saved, unSavedItem));

            if (savedItem is null)
            {
                returnList.Add(unSavedItem);
            }
            else
            {
                unSavedItem.Id = savedItem.Id;
                savedItem = mapper.Map(unSavedItem, savedItem);
                returnList.Add(savedItem);
            }
        }

        return returnList;
    }

    public static bool IsValidUrl(string url)
    {
        return string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }

    //return distance in meters https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
    public static double GetDistance(double? latitude1, double? longitude1, double? latitude2, double? longitude2, string? name = null)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Debug.WriteLine(name);
        }

        latitude1 ??= 0.0;
        longitude1 ??= 0.0;
        latitude2 ??= 0.0;
        longitude2 ??= 0.0;

        var pin1 = new GeoCoordinate(latitude1.Value, longitude1.Value);
        var pin2 = new GeoCoordinate(latitude2.Value, longitude2.Value);

        return pin1.GetDistanceTo(pin2);
    }
}