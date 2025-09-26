using Api.Entities;
using Api.Pagination;

namespace Api.Repositories;

public interface ITweetRepository
{
    Task AddTweet(TweetEntity entity);
    Task DeleteTweet(TweetEntity entity);
    Task<List<TweetEntity>> GetTweetsFromBox(Guid boxId, PageRequestDto page);
    Task<TweetEntity?> GetLatestTweet(Guid boxId);
}
