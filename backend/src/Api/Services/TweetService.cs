public sealed class TweetService(ITweetRepository tweetRepository) : ITweetService
{
    public async Task<TweetEntity> GetTweetById(Guid id)
    {
        await tweetRepository.GetTweetById(id);
    }

    public async Task AddTweet(TweetEntity entity)
    {
        await tweetRepository.AddTweet(entity);
    }

    public async Task DeleteTweet(TweetEntity entity)
    {
        await tweetRepository.DeleteTweet(entity);
    }
}