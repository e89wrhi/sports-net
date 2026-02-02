using Microsoft.EntityFrameworkCore;
using Sport.Common.EFCore;

namespace Vote.Data.Seed;

public class VoteDataSeeder(
    VoteDbContext voteDbContext
) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        var pendingMigrations = await voteDbContext.Database.GetPendingMigrationsAsync();

        if (!pendingMigrations.Any())
        {
            await SeedVoteAsync();
        }
    }

    private async Task SeedVoteAsync()
    {
        if (!await voteDbContext.Votes.AnyAsync())
        {
            await voteDbContext.Votes.AddRangeAsync(InitialData.Votes);
            await voteDbContext.SaveChangesAsync();
        }
    }
}