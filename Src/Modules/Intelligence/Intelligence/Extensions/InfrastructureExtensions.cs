using Sport.Common.EFCore;
using Sport.Common.Mapster;
using Sport.Common.Web;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace Intelligence.Extensions;


public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddIntelligenceModules(this WebApplicationBuilder builder)
    {
        builder.AddMinimalEndpoints(assemblies: typeof(SimpleMDRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(SimpleMDRoot).Assembly);
        builder.Services.AddCustomMapster(typeof(SimpleMDRoot).Assembly);

        builder.Services.AddCustomMediatR();

        return builder;
    }


    public static WebApplication UseIntelligenceModules(this WebApplication app)
    { 
        return app;
    }
}