using Api.DTOs;
using Api.Services;

namespace Api.Endpoints;

public static class BoxEndpoints
{
    public static RouteGroupBuilder MapBoxEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/boxes")
            .WithTags("Boxes");

        group.MapGet("/{boxId}", GetCustomerById)
            .WithName(nameof(GetCustomerById))
            .Produces<BoxResponse>(StatusCodes.Status200OK);

        group.MapGet("/", GetCustomerIds)
            .Produces<IEnumerable<BoxResponse>>(StatusCodes.Status200OK);

        group.MapPost("/", CreateCustomer)
            .Produces<BoxResponse>(StatusCodes.Status201Created);

        group.MapPut("/{boxId}", UpdateCustomer)
            .Produces<BoxResponse>(StatusCodes.Status204NoContent);

        group.MapDelete("/{boxId}", DeleteCustomer)
            .Produces<BoxResponse>(StatusCodes.Status204NoContent);

        return group;
    }

}
