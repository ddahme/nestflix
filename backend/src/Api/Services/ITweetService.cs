using Api.DTOs;

namespace Api.Services;

public interface ITweetService
{
    Task UploadTweet(CreateTweetRequest request);
    Task<TweetResponse> GetTweetById(Guid id);
    Task<IEnumerable<TweetResponse>> GetTweetsFromBox(Guid boxId);
}
