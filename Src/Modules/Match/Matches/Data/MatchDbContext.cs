using Sport.Common.EFCore;
using Matches.Matches.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Matches.Data;

using Sport.Common.Web;
using Microsoft.Extensions.Logging;
using Sport.Common.EFCore;

public sealed class MatchDbContext : AppDbContextBase
{
    public MatchDbContext(DbContextOptions<MatchDbContext> options, ICurrentUserProvider? currentUserProvider = null,
        ILogger<MatchDbContext>? logger = null) : base(
        options, currentUserProvider, logger)
    {
    }

    public DbSet<MatchModel> Matches => Set<MatchModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.FilterSoftDeletedProperties();
        builder.ToSnakeCaseTables();
    }
}