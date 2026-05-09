using Event;
using Figgle;
using Identity;
using Match;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Sport;
using Sport.Common.AI;
using Sport.Common.BaseExceptions;
using Sport.Common.Core;
using Sport.Common.Jwt;
using Sport.Common.MassTransit;
using Sport.Common.OpenApi;
using Sport.Common.PersistMessageProcessor;
using Sport.Common.Problems;
using Sport.Common.Web;
using Vote;

namespace Api.Extensions;

public static class SharedInfrastructureExtensions
{
    public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
    {
        var appOptions = builder.Services.GetOptions<AppOptions>(nameof(AppOptions));
        Console.WriteLine(FiggleFonts.Standard.Render(appOptions.Name));

        builder.AddServiceDefaults();

        builder.Services.AddJwt();
        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddTransient<AuthHeaderHandler>();
        builder.AddPersistMessageProcessor();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddAspnetOpenApi();
        builder.Services.AddCustomVersioning();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

        builder.Services.AddCustomAI(builder.Configuration);
        builder.Services.AddCustomMassTransit(
            builder.Environment,
            TransportType.InMemory,
            AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.Configure<ApiBehaviorOptions>(
            options => options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddGrpc(
            options =>
            {
                options.Interceptors.Add<GrpcExceptionInterceptor>();
            });

        builder.Services.AddEasyCaching(options => { options.UseInMemory(builder.Configuration, "mem"); });
        builder.Services.AddProblemDetails();

        builder.Services.AddScoped<IEventMapper>(sp =>
        {
            var mappers = new IEventMapper[] {
                                                 sp.GetRequiredService<EventEventMapper>(),
                                                 sp.GetRequiredService<IdentityEventMapper>(),
                                                 sp.GetRequiredService<MatchEventMapper>(),
                                                 sp.GetRequiredService<VoteEventMapper>(),
                                             };

            return new CompositeEventMapper(mappers);
        });


        return builder;
    }


    public static WebApplication UserSharedInfrastructure(this WebApplication app)
    {
        var appOptions = app.Configuration.GetOptions<AppOptions>(nameof(AppOptions));

        app.UseServiceDefaults();

        app.UseCustomProblemDetails();

        app.UseCorrelationId();

        app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

        if (app.Environment.IsDevelopment())
        {
            app.UseAspnetOpenApi();
        }

        app.MapGet("/debug-api", (Microsoft.AspNetCore.Mvc.ApiExplorer.IApiDescriptionGroupCollectionProvider provider) => 
        {
            return provider.ApiDescriptionGroups.Items.Select(g => new { g.GroupName, Count = g.Items.Count });
        });

        return app;
    }
}