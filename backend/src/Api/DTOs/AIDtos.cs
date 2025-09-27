namespace Api.DTOs;

public sealed record AIRequest
{
    public required Stream Image { get; init; }
}

public sealed record AIResponse
{
    public required bool IsOccupied { get; init; }
    public required string BirdType { get; init; }
    public required int EggCount { get; init; }
    public required int HatchedCount { get; init; }
}