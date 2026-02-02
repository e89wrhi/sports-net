using Microsoft.EntityFrameworkCore;
using Sport.Common.EFCore;

namespace Match.Data.Seed;

public class MatchDataSeeder(
    MatchDbContext matchDbContext
) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        var pendingMigrations = await matchDbContext.Database.GetPendingMigrationsAsync();

        if (!pendingMigrations.Any())
        {
            await SeedMatchAsync();
        }
    }

    private async Task SeedMatchAsync()
    {
        if (!await matchDbContext.Matches.AnyAsync())
        {
            await matchDbContext.Matches.AddRangeAsync(InitialData.Matchs);
            await matchDbContext.SaveChangesAsync();
        }
    }
} 