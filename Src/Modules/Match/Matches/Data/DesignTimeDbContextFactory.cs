using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Match.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MatchDbContext>
{
    public MatchDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<MatchDbContext>();

        builder.UseNpgsql("Server=localhost;Port=5431;Database=match;User Id=postgres;Password=changeme;Include Error Detail=true")
            .UseSnakeCaseNamingConvention();
        return new MatchDbContext(builder.Options);
    }
}