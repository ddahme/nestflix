namespace Api.Endpoints;

public static class TweetEndpoints
{

    public static RouteGroupBuilder MapTweetEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/boxes/{boxId:guid}/tweets")
            .WithTags("Tweets");

        group.MapGet("/{tweetId}", GetTweetById)
            .WithName(nameof(GetTweetById))
            .Produces<TweetEntity>(StatusCodes.Status200OK);

        group.MapGet("/", DeleteTweet)
            .WithName(nameof(DeleteTweet))
            .Produces<TweetResponse>(StatusCodes.Status200OK);

        group.MapPost("/", AddTweet)
            .Produces<TweetResponse>(StatusCodes.Status201Created);

        return group;
    }

}
