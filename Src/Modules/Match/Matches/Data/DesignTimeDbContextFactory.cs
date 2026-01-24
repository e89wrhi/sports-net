using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Matches.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MatchDbContext>
{
    public MatchDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<MatchDbContext>();

        builder.UseNpgsql("Server=localhost;Port=5432;Database=match;User Id=postgres;Password=postgres;Include Error Detail=true")
            .UseSnakeCaseNamingConvention();
        return new MatchDbContext(builder.Options);
    }
}