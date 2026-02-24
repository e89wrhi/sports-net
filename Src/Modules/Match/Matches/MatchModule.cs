using Sport.Common.Web;
using Match.Extensions.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace Match;

public class MatchModule : IModule
{
    public WebApplicationBuilder AddModule(WebApplicationBuilder builder)
    {
        return builder.AddMatchModules();
    }

    public WebApplication UseModule(WebApplication app)
    {
        return app.UseMatchModules();
    }
}
