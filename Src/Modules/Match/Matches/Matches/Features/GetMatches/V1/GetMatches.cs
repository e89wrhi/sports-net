using Duende.IdentityServer.EntityFramework.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sport.Common.Web;

namespace Match.Features.GetMatches.V1;

public class GetMatchsEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/match/get-matches?leagues={{league}}&status={{status}}",
                async (IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetMatchs(), cancellationToken);

                    var response = result.Adapt<GetMatchsResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("GetMatchs")
            .WithApiVersionSet(builder.NewApiVersionSet("Match").Build())
            .Produces<GetMatchsResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Matchs")
            .WithDescription("Get Matchs")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
