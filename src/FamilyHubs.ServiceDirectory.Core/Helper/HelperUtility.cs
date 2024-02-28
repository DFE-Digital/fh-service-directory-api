﻿using System.Diagnostics;
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
    //todo: needed?
    //public static void AttachExistingManyToMany(this Organisation organisation, ApplicationDbContext context, IMapper mapper)
    //{
    //    //todo: match on name, address1 & postcode?
    //    var existingLocations = organisation.Location.Select(s => $"{s.Name}{s.PostCode}").ToList();
    //    organisation.Location = organisation.Location.AddOrAttachExisting(context, mapper,
    //        l => existingLocations.Contains(l.Name + l.PostCode),
    //        (s, d) => $"{s.Name}{s.PostCode}" == $"{d.Name}{d.PostCode}");
    //}

    //todo: this can be generic on type, and can we use it for taxonomy too?
    public static void AttachExisting(this Location location, ApplicationDbContext context, IMapper mapper)
    {
        if (location.Id != 0)
        {
            var existingLocation = context.Locations.Find(location.Id);
            if (existingLocation != null)
            {
                mapper.Map(location, existingLocation);
                context.Entry(existingLocation).State = EntityState.Modified;
            }
            else
            {
                throw new NotFoundException(nameof(Location), location.Id.ToString());
            }
        }
        else
        {
            context.Locations.Add(location);
        }
    }

    //todo: should this be attaching by id instead? will have to check assumptions of the existing consumers
    public static void AttachExistingManyToMany(this Service service, ApplicationDbContext context, IMapper mapper)
    {
        //todo: match on name, address1 & postcode?
        // or use contains? (i.e. check everything??)
        //var existingLocations = service.Locations.Select(s => $"{s.Name}{s.PostCode}").ToList();
        //service.Locations = service.Locations.AddOrAttachExisting(context, mapper,
        //    l => existingLocations.Contains(l.Name + l.PostCode),
        //    (s, d) => $"{s.Name}{s.PostCode}" == $"{d.Name}{d.PostCode}");

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