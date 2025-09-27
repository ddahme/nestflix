using Api.DTOs;
using Api.Pagination;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class BoxEndpoints
{
    public static RouteGroupBuilder MapBoxEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/boxes")
            .WithTags("Boxes");

        group.MapGet("/{boxId}", GetBoxById)
            .Produces<BoxResponse>(StatusCodes.Status200OK);

        group.MapGet("/", GetBoxesInDistance)
            .Produces<IEnumerable<BoxResponse>>(StatusCodes.Status200OK);

        group.MapPost("/", CreateBox)
            .Produces<BoxResponse>(StatusCodes.Status201Created);

        group.MapPut("/{boxId}", UpdateBox)
            .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{boxId}", ArchiveBox)
            .Produces(StatusCodes.Status204NoContent);

        return group;
    }

    private static async Task<IResult> ArchiveBox(Guid boxId, IBoxService boxService)
    {
        await boxService.ArchiveBox(boxId);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateBox(Guid boxId, UpdateBoxRequest request, IBoxService boxService)
    {
        await boxService.UpdateBox(boxId, request);
        return Results.NoContent();
    }

    private static async Task<IResult> CreateBox(CreateBoxRequest request, IBoxService boxService)
    {
        await boxService.CreateBox(request);

        return Results.Created();
    }

    private static async Task<IResult> GetBoxesInDistance([AsParameters] PointDto point, [AsParameters] PageRequestDto page, [FromQuery] double distanceInMeters, IBoxService boxService)
    {
        GetBoxesInDistanceRequest request = new()
        {
            Point = point,
            DistanceInMeters = distanceInMeters,
            Page = page
        };
        return Results.Ok(await boxService.GetBoxesInDistance(request));
    }

    private static async Task<IResult> GetBoxById(Guid boxId, IBoxService boxService)
    {
        return Results.Ok(await boxService.GetBoxById(boxId));
    }
}
