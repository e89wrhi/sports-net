using Match.Matches.Data;
using Match.Matches.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sport.Common.Core.CQRS;
using Sport.Common.Web;

namespace Match.Matches.Features.GetMatches.V1;

public record GetMatchesQuery() : IQuery<MatchesDto>;

public class GetMatchesEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/v1/matches", async (ISender sender) =>
        {
            var result = await sender.Send(new GetMatchesQuery());
            return Results.Ok(result);
        })
        .WithName("GetMatches")
        .WithTags("Matches")
        .Produces<MatchesDto>(StatusCodes.Status200OK);

        return builder;
    }
}

public class GetMatchesHandler : IQueryHandler<GetMatchesQuery, MatchesDto>
{
    private readonly IMatchRepository _repository;

    public GetMatchesHandler(IMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<MatchesDto> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
    {
        var matches = await _repository.GetAllAsync(cancellationToken);

        var dtos = matches.Select(match => new MatchDto(
            match.Id.Value,
            match.HomeTeam.Value,
            match.AwayTeam.Value,
            match.HomeTeamScore?.Value ?? 0,
            match.AwayTeamScore?.Value ?? 0,
            match.League,
            match.Status,
            match.MatchTime,
            match.EventsCount,
            match.HomeVotesCount,
            match.AwayVotesCount,
            match.DrawVotesCount));

        return new MatchesDto(dtos);
    }
}
