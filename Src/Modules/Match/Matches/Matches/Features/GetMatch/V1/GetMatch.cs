using Match.Matches.Data;
using Match.Matches.Dtos;
using Match.Matches.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sport.Common.Core.CQRS;
using Sport.Common.Web;
using Sport.Common.Exception;

namespace Match.Matches.Features.GetMatch.V1;

public record GetMatchQuery(Guid Id) : IQuery<MatchDto>;

public class GetMatchEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/v1/matches/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetMatchQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetMatch")
        .WithTags("Matches")
        .Produces<MatchDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        return builder;
    }
}

public class GetMatchHandler : IQueryHandler<GetMatchQuery, MatchDto>
{
    private readonly IMatchRepository _repository;

    public GetMatchHandler(IMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<MatchDto> Handle(GetMatchQuery request, CancellationToken cancellationToken)
    {
        var match = await _repository.FindByIdAsync(MatchId.Of(request.Id), cancellationToken);

        if (match == null)
        {
            throw new NotFoundException($"Match with id {request.Id} not found.");
        }

        return new MatchDto(
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
            match.DrawVotesCount);
    }
}
