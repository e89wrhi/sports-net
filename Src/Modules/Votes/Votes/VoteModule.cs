using Sport.Common.Web;
using Vote.Extensions.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace Vote;

public class VoteModule : IModule
{
    public WebApplicationBuilder AddModule(WebApplicationBuilder builder)
    {
        return builder.AddVoteModules();
    }

    public WebApplication UseModule(WebApplication app)
    {
        return app.UseVoteModules();
    }
}
