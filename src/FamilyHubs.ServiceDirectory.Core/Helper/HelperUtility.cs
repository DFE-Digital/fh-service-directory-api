using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Data.Repository;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace FamilyHubs.ServiceDirectory.Core.Helper;

//todo: move code out of this generic helper class
public static class HelperUtility
{
    // this matches the validation we have in the frontend
    private static readonly Regex IsValidUrlRegEx = new(
        @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    //public static async Task<TEntity> GetEntity<TEntity>(
    //    this long id,
    //    DbSet<TEntity> existingEntities)
    //    where TEntity : EntityBase<long>
    //{
    //    var foundEntity = await existingEntities.FindAsync(id);
    //    if (foundEntity == null)
    //    {
    //        throw new NotFoundException(id.ToString(), nameof(TEntity));
    //    }

    //    return foundEntity;
    //}

    public static async Task<List<TEntity>> GetEntities<TEntity>(
        this ICollection<long> ids,
        DbSet<TEntity> existingEntities)
        where TEntity : EntityBase<long>
    {
        var findTasks = ids.Select(id => existingEntities.FindAsync(id).AsTask());

        var foundEntities = await Task.WhenAll(findTasks);

        if (foundEntities.Any(e => e == null))
        {
            //todo: this Ardalis exception doesn't seem to support multiple objects
            //todo: NotFoundException in other commands use either Ardalis or a custom exception
            //todo: the other Ardalis throws seem to have the params the wrong way round
            throw new NotFoundException("Many", nameof(Location));
        }
        return foundEntities.ToList()!;
    }

    //todo: generic for entity
    public static async Task<List<TEntity>> LinkExistingEntities<TEntity>(
        this IList<TEntity> entities,
        DbSet<TEntity> existingEntities,
        IMapper mapper,
        bool mapExistingEntity = true)
        where TEntity : EntityBase<long>
    {
        List<TEntity> linkedEntities = new();
        foreach (TEntity entity in entities)
        {
            TEntity newEntity;
            //or IsKeySet
            if (entity.Id != 0)
            {
                TEntity existingLocation = await existingEntities.FindAsync(entity.Id)
                    ?? throw new NotFoundException(nameof(TEntity), entity.Id.ToString());

                if (mapExistingEntity)
                {
                    mapper.Map(entity, existingLocation);
                }

                newEntity = existingLocation;
            }
            else
            {
                newEntity = entity;
            }

            linkedEntities.Add(newEntity);
        }

        return linkedEntities;
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
        return IsValidUrlRegEx.IsMatch(url);
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