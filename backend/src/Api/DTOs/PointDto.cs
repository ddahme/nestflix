namespace Api.DTOs;

public sealed record PointDto
{
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}
