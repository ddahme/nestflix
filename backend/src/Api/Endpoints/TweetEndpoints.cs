namespace Api.Endpoints;

public static class TweetEndpoints
{

    public static RouteGroupBuilder MapTweetEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/boxes/{boxId:guid}/tweets")
            .WithTags("Tweets");

        group.MapGet("/{tweetId}", GetCustomerById)
            .WithName(nameof(GetCustomerById))
            .Produces<TweetResponse>(StatusCodes.Status200OK);

        group.MapGet("/", GetCustomerIds)
            .Produces<IEnumerable<TweetResponse>>(StatusCodes.Status200OK);

        group.MapPost("/", CreateCustomer)
            .Produces<TweetResponse>(StatusCodes.Status201Created);

        return group;
    }

}
