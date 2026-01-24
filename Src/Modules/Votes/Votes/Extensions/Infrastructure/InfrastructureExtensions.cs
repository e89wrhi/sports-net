using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sport.Common.EFCore;
using Sport.Common.Mapster;
using Sport.Common.Mongo;
using Sport.Common.Web;
using Vote.Data;
using Vote.Data.Seed;

namespace Vote.Extensions.Infrastructure;


public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddVoteModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<VoteEventMapper>();
        builder.AddMinimalEndpoints(assemblies: typeof(VoteRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(VoteRoot).Assembly);
        builder.Services.AddCustomMapster(typeof(VoteRoot).Assembly);
        builder.AddCustomDbContext<VoteDbContext>(nameof(Vote));
        builder.Services.AddScoped<IDataSeeder, VoteDataSeeder>();
        builder.AddMongoDbContext<VoteReadDbContext>();

        builder.Services.AddCustomMediatR();

        return builder;
    }


    public static WebApplication UseVoteModules(this WebApplication app)
    {
        app.UseMigration<VoteDbContext>();
        app.MapGrpcService<VoteGrpcServices>();

        return app;
    }
}