using Sport.Common.EFCore;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Votes.Votes.Models;

namespace Votes.Data.Seed;

public class VoteDataSeeder(
    VoteDbContext voteDbContext,
    VoteReadDbContext voteReadDbContext,
    IMapper mapper
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
        if (!await EntityFrameworkQueryableExtensions.AnyAsync(voteDbContext.Votes))
        {
            await voteDbContext.Votes.AddRangeAsync(InitialData.Votes);
            await voteDbContext.SaveChangesAsync();

            if (!await MongoQueryable.AnyAsync(voteReadDbContext.Vote.AsQueryable()))
            {
                await voteReadDbContext.Vote.InsertManyAsync(mapper.Map<List<VoteReadModel>>(InitialData.Votes));
            }
        }
    }
}