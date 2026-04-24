using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Vote.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VoteDbContext>
{
    public VoteDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<VoteDbContext>();

        builder.UseNpgsql("Server=localhost;Port=5431;Database=vote;User Id=postgres;Password=changeme;Include Error Detail=true")
            .UseSnakeCaseNamingConvention();
        return new VoteDbContext(builder.Options);
    }
} 