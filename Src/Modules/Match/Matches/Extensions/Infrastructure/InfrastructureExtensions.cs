using FluentValidation;
using Match.Data;
using Match.Data.Seed;
using Match.GrpcServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sport.Common.EFCore;
using Sport.Common.Mapster;
using Sport.Common.Web;

namespace Match.Extensions.Infrastructure;


public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddMatchModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<MatchEventMapper>();
        builder.AddMinimalEndpoints(assemblies: typeof(MatchRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(MatchRoot).Assembly);
        builder.Services.AddCustomMapster(typeof(MatchRoot).Assembly);
        builder.AddCustomDbContext<MatchDbContext>(nameof(Match));
        builder.Services.AddScoped<IDataSeeder, MatchDataSeeder>();

        builder.Services.AddCustomMediatR();

        return builder;
    }


    public static WebApplication UseMatchModules(this WebApplication app)
    {
        app.UseMigration<MatchDbContext>();
        app.MapGrpcService<MatchGrpcServices>();

        return app;
    }
}