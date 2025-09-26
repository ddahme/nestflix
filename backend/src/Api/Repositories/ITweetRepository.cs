using Api.Entities;

namespace Api.Repositories;

public interface ITweetRepository
{
    Task AddTweet(TweetEntity entity);
    Task DeleteTweet(TweetEntity entity);
    Task<TweetEntity> GetTweetById(Guid id);
}
