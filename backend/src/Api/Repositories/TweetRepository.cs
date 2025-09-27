using Api.Database;
using Api.Entities;
using Api.Pagination;
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
        context.Tweets.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<TweetEntity?> GetLatestTweet(Guid boxId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Tweets
            .Where(t => t.BoxId == boxId)
            .OrderByDescending(t => t.UploadedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<List<TweetEntity>> GetTweetsFromBox(Guid boxId, PageRequestDto page)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Tweets
            .Where(t => t.BoxId == boxId)
            .OrderByDescending(t => t.UploadedAt)
            .Skip(page.Offest())
            .Take(page.Size)
            .ToListAsync();
    }
}
