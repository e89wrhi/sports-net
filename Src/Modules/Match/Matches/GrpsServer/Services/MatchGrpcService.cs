using System.Threading.Tasks;
using Grpc.Core;
using Mapster;
using MediatR;
using match;
using Match.Features.GetMatch.V1;

namespace Match.GrpcServer.Services;

using Grpc.Core;
using System;

public class MatchGrpcServices : MatchGrpcService.MatchGrpcServiceBase
{
    private readonly IMediator _mediator;

    public MatchGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetMatchByIdResult> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetMatchById(Guid.Parse(request.Id)));

        return new GetMatchByIdResult
        {
            MatchDto = new MatchResponse
            {
                Id = result.MatchDto.Id.ToString(),
                HomeTeam = result.MatchDto.HomeTeam,
                AwayTeam = result.MatchDto.AwayTeam,
                HomeTeamScore = result.MatchDto.HomeTeamScore,
                AwayTeamScore = result.MatchDto.AwayTeamScore,
                League = result.MatchDto.League.ToString(),
                Status = result.MatchDto.Status.ToString(),
                MatchTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.MatchDto.MatchTime.ToUniversalTime()),
                EventsCount = result.MatchDto.EventsCount,
                HomeVotesCount = result.MatchDto.HomeVotesCount,
                AwayVotesCount = result.MatchDto.AwayVotesCount,
                DrawVotesCount = result.MatchDto.DrawVotesCount
            }
        };
    }

}