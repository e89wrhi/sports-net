using Duende.IdentityServer.EntityFramework.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sport.Common.Web;

namespace Events.Features.GetEvents.V1;

public class GetEventsEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/event/get-events",
                async (IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetEvents(), cancellationToken);

                    var response = result.Adapt<GetEventsResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("GetEvents")
            .WithApiVersionSet(builder.NewApiVersionSet("Event").Build())
            .Produces<GetEventsResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Events")
            .WithDescription("Get Events")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
