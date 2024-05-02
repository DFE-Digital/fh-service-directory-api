using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using NetTopologySuite.Geometries;
using Location = FamilyHubs.ServiceDirectory.Data.Entities.Location;

namespace FamilyHubs.ServiceDirectory.Core.Helper;

public class GeoPoint
{
    public const int WGS84 = 4326;

    public class Resolver : IValueResolver<LocationDto, Location, Point>
    {
        public Point Resolve(LocationDto source, Location destination, Point destMember, ResolutionContext context)
        {
            return new Point(source.Longitude, source.Latitude) { SRID = GeoPoint.WGS84 };
        }
    }
}
