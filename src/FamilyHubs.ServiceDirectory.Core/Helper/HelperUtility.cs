using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FamilyHubs.ServiceDirectory.Core.Helper;

//todo: move code out of this generic helper class
public static class HelperUtility
{
    // this matches the validation we have in the frontend
    private static readonly Regex IsValidUrlRegEx = new(
        @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static async Task<List<TEntity>> GetEntities<TEntity>(
        this ICollection<long> ids,
        DbSet<TEntity> existingEntities)
        where TEntity : EntityBase<long>
    {
        List<TEntity> foundEntities = new();

        foreach (var id in ids)
        {
            var existingEntity = await existingEntities.FindAsync(id);
            if (existingEntity == null)
            {
                //todo: this Ardalis exception doesn't seem to support multiple objects
                //todo: NotFoundException in other commands use either Ardalis or a custom exception
                //todo: the other Ardalis throws seem to have the params the wrong way round
                throw new NotFoundException(id.ToString(), nameof(Location));
            }
            foundEntities.Add(existingEntity);
        }

        return foundEntities;
    }

    public static bool IsValidUrl(string url)
    {
        return IsValidUrlRegEx.IsMatch(url);
    }

    //todo: throw this away and do it in the db using EF, see https://learn.microsoft.com/en-us/ef/core/modeling/spatial

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