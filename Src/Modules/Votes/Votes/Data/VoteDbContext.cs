using Sport.Common.EFCore;
using Vote.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Vote.Data;

using Sport.Common.Web;
using Microsoft.Extensions.Logging;
using Sport.Common.EFCore;

public sealed class VoteDbContext : AppDbContextBase
{
    public VoteDbContext(DbContextOptions<VoteDbContext> options, ICurrentUserProvider? currentUserProvider = null,
        ILogger<VoteDbContext>? logger = null) : base(
        options, currentUserProvider, logger)
    {
    }

    public DbSet<VoteModel> Votes => Set<VoteModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.FilterSoftDeletedProperties();
        builder.ToSnakeCaseTables();
    }
}