using Events.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Sport.Common.EFCore;

namespace Event.Data.Seed;

public class EventDataSeeder(
    EventDbContext eventDbContext
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
        if (!await eventDbContext.Events.AnyAsync())
        {
            await eventDbContext.Events.AddRangeAsync(InitialData.Events);
            await eventDbContext.SaveChangesAsync();
        }
    }
}