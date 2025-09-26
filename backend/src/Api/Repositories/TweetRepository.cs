using Api.Database;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public sealed class TweetRepository(IDbContextFactory<NestflixDbContext> dbContextFactory) : ITweetRepository
{

    public async Task AddTweet(TweetEntity entity)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        await context.Tweets.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTweet(TweetEntity entity)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        await context.Attach(entity);
        await context.Tweets.Remove();
        await context.SaveChangesAsync();

    }

    public Task<TweetEntity> GetTweetById(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();

        var tweet = await db.Tweets.FirstOrDefaultAsync(t => t.Id == id);

        return tweet is null ? throw new KeyNotFoundException($"Tweet with id {id} was not found.") : tweet;
    }
}
