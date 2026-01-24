using Sport.Common.EFCore;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Matches.Matches.Models;

namespace Matches.Data.Seed;

public class MatchDataSeeder(
    MatchDbContext matchDbContext,
    MatchReadDbContext matchReadDbContext,
    IMapper mapper
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
        if (!await EntityFrameworkQueryableExtensions.AnyAsync(matchDbContext.Matches))
        {
            await matchDbContext.Matches.AddRangeAsync(InitialData.Matchs);
            await matchDbContext.SaveChangesAsync();

            if (!await MongoQueryable.AnyAsync(matchReadDbContext.Match.AsQueryable()))
            {
                await matchReadDbContext.Match.InsertManyAsync(mapper.Map<List<MatchReadModel>>(InitialData.Matchs));
            }
        }
    }
} 