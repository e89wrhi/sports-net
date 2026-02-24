using Sport.Common.Web;
using Event.Extensions;
using Microsoft.AspNetCore.Builder;
using Event.Extensions.Infrastructure;

namespace Event;

public class EventModule : IModule
{
    public WebApplicationBuilder AddModule(WebApplicationBuilder builder)
    {
        return builder.AddEventModules();
    }

    public WebApplication UseModule(WebApplication app)
    {
        return app.UseEventModules();
    }
}
