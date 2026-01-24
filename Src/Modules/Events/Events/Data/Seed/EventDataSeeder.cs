using Sport.Common.EFCore;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Event.Events.Models;

namespace Event.Data.Seed;

public class EventDataSeeder(
    EventDbContext eventDbContext,
    EventReadDbContext eventReadDbContext,
    IMapper mapper
) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        var pendingMigrations = await eventDbContext.Database.GetPendingMigrationsAsync();

        if (!pendingMigrations.Any())
        {
            await SeedEventAsync();
        }
    }

    private async Task SeedEventAsync()
    {
        if (!await EntityFrameworkQueryableExtensions.AnyAsync(eventDbContext.Events))
        {
            await eventDbContext.Events.AddRangeAsync(InitialData.Events);
            await eventDbContext.SaveChangesAsync();

            if (!await MongoQueryable.AnyAsync(eventReadDbContext.Event.AsQueryable()))
            {
                await eventReadDbContext.Event.InsertManyAsync(mapper.Map<List<EventReadModel>>(InitialData.Events));
            }
        }
    }
}