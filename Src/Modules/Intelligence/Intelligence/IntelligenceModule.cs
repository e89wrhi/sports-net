using Sport.Common.Web;
using Intelligence.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Intelligence;

public class IntelligenceModule : IModule
{
    public WebApplicationBuilder AddModule(WebApplicationBuilder builder)
    {
        return builder.AddIntelligenceModules();
    }

    public WebApplication UseModule(WebApplication app)
    {
        return app.UseIntelligenceModules();
    }
}
