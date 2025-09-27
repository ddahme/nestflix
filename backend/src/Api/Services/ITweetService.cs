using Api.DTOs;
using Api.Pagination;

namespace Api.Services;

public interface ITweetService
{
    Task UploadTweet(Guid boxId, CreateTweetRequest request);
    Task<IEnumerable<TweetResponse>> GetTweetsFromBox(Guid boxId, PageRequestDto page);
}
