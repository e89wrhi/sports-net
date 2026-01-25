using Event.Extensions.Infrastructure;
using Event.Data;
using Event.Data.Seed;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sport.Common.EFCore;
using Sport.Common.Mapster;
using Sport.Common.Mongo;
using Sport.Common.Web;
using Event.GrpcServer.Services;

namespace Event.Extensions.Infrastructure;


public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddEventModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<EventEventMapper>();
        builder.AddMinimalEndpoints(assemblies: typeof(EventRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(EventRoot).Assembly);
        builder.Services.AddCustomMapster(typeof(EventRoot).Assembly);
        builder.AddCustomDbContext<EventDbContext>(nameof(Event));
        builder.Services.AddScoped<IDataSeeder, EventDataSeeder>();
        builder.AddMongoDbContext<EventReadDbContext>();
        
        builder.Services.AddCustomMediatR();

        return builder;
    }


    public static WebApplication UseEventModules(this WebApplication app)
    {
        app.UseMigration<EventDbContext>();
        app.MapGrpcService<EventGrpcServices>();

        return app;
    }
}