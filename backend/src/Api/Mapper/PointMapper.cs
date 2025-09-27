using Api.DTOs;
using NetTopologySuite.Geometries;

namespace Api.Mapper;

public static class PointMapper
{
    public static Point ToPoint(this PointDto point)
    {
        return new Point(point.Longitude, point.Latitude) { SRID = 4326 };
    }

    public static PointDto ToPointDto(this Point point)
    {
        return new()
        {
            Latitude = point.Y,
            Longitude = point.X
        };
    }
}
