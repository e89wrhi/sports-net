using Sport.Common.EFCore;
using Event.Events.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Event.Data;

using Sport.Common.Web;
using Microsoft.Extensions.Logging;
using Sport.Common.EFCore;

public sealed class EventDbContext : AppDbContextBase
{
    public EventDbContext(DbContextOptions<EventDbContext> options, ICurrentUserProvider? currentUserProvider = null,
        ILogger<EventDbContext>? logger = null) : base(
        options, currentUserProvider, logger)
    {
    }

    public DbSet<EventModel> Events => Set<EventModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.FilterSoftDeletedProperties();
        builder.ToSnakeCaseTables();
    }
}