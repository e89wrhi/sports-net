using Sport.Common.Web;
using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Intelligence.Features.PredictMatch.V1;

public class PredictMatchEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/intelligence/predict",
                async (PredictMatchRequestDto request, IMediator mediator, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken) =>
                {
                    // current user id
                    var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (!Guid.TryParse(userIdClaim, out var userId))
                    {
                        return Results.Unauthorized();
                    }

                    // here aggregate match info + events + votes here
                    var command = new PredictMatchCommand(
                        HomeTeam: "",
                        AwayTeam: "",
                        HomeScore: 0,
                        AwayScore: 0,
                        Minute: 0,
                        Events: new List<MatchEventDto>(),
                        HomeTeamInfo: new TeamInfoDto(Name: "", Ranking: 0, Form: "", IsHome: false),
                        AwayTeamInfo: new TeamInfoDto(Name: "", Ranking: 0, Form: "", IsHome: false),
                        Votes: new VoteStatsDto(HomeWinVotes: 0, DrawVotes: 0, AwayWinVotes: 0),
                        request.ModelId);
                    var result = await mediator.Send(command, cancellationToken);
                    return Results.Ok(new PredictMatchResponseDto(
                        result.Prediction,
                        result.HomeWinProbability,
                        result.DrawProbability,
                        result.AwayWinProbability,
                        result.ModelId, 
                        result.ProviderName));
                })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("PredictMatch")
            .WithApiVersionSet(builder.NewApiVersionSet("PredictMatch").Build())
            .Produces<PredictMatchResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest).ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Predict Match")
            .WithDescription("Predict Match Outcome using AI.")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
