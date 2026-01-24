using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Events.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
{
    public EventDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<EventDbContext>();

        builder.UseNpgsql("Server=localhost;Port=5432;Database=event;User Id=postgres;Password=postgres;Include Error Detail=true")
            .UseSnakeCaseNamingConvention();
        return new EventDbContext(builder.Options);
    }
}