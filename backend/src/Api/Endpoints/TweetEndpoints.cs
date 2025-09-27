using Api.DTOs;
using Api.Pagination;
using Api.Services;

namespace Api.Endpoints;

public static class TweetEndpoints
{

    public static RouteGroupBuilder MapTweetEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/boxes/{boxId:guid}/tweets")
            .WithTags("Tweets");

        group.MapGet("/", GetTweetsFromBox)
            .Produces<IEnumerable<TweetResponse>>(StatusCodes.Status200OK);


        group.MapPost("/", UploadTweet)
            .Produces(StatusCodes.Status201Created);

        return group;
    }

    private static async Task<IResult> UploadTweet(Guid boxId, CreateTweetRequest request, ITweetService tweetService)
    {
        await tweetService.UploadTweet(boxId, request);
        return Results.Created();
    }

    private static async Task<IResult> GetTweetsFromBox(Guid boxId, [AsParameters] PageRequestDto page, ITweetService tweetService)
    {
        IEnumerable<TweetResponse> tweets = await tweetService.GetTweetsFromBox(boxId, page);
        return Results.Ok(tweets);
    }
}
