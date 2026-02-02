using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Match.Data;
using Match.Dtos;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Caching;
using Sport.Common.Core;
using Sport.Common.Web;
using Sport.Matchs.Exceptions;
using Match.Enums;

namespace Match.Features.GetMatches.V1;

public record GetMatchs : IQuery<GetMatchsResult>, ICacheRequest
{
    public MatchLeague League;
    public MatchStatus Status;
    public string CacheKey => "GetMatchs";
    public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddHours(1);
}

public record GetMatchsResult(IEnumerable<MatchDto> MatchDtos);

public record GetMatchsResponseDto(IEnumerable<MatchDto> MatchDtos);

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

internal class GetMatchsHandler : IQueryHandler<GetMatchs, GetMatchsResult>
{
    private readonly IMapper _mapper;
    private readonly MatchDbContext _matchDbContext;

    public GetMatchsHandler(IMapper mapper, MatchDbContext matchDbContext)
    {
        _mapper = mapper;
        _matchDbContext = matchDbContext;
    }

    public async Task<GetMatchsResult> Handle(GetMatchs request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var match = (await _matchDbContext.Match.AsQueryable().ToListAsync(cancellationToken))
            .Where(x => !x.IsDeleted);

        if (!match.Any())
        {
            throw new MatchNotFoundException(Guid.Empty);
        }

        var matchDtos = _mapper.Map<IEnumerable<MatchDto>>(match);

        return new GetMatchsResult(matchDtos);
    }
}