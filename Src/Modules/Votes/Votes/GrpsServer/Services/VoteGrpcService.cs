using System.Threading.Tasks;
using Grpc.Core;
using Mapster;
using MediatR;
using vote;
using Vote.Votes.Features.GetVoteById.V1;

namespace Vote.GrpcServer.Services;

using Grpc.Core;
using System;

public class VoteGrpcServices : VoteGrpcService.VoteGrpcServiceBase
{
    private readonly IMediator _mediator;

    public VoteGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetVoteByIdResult> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetVoteById(Guid.Parse(request.Id)));

        return new GetVoteByIdResult
        {
            VoteDto = new VoteResponse
            {
                Id = result.VoteDto.Id.ToString(),
                VoterId = result.VoteDto.VoterId.ToString(),
                MatchId = result.VoteDto.MatchId.ToString(),
                Type = result.VoteDto.Type,
                VotedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.VoteDto.VotedAt.ToUniversalTime())
            }
        };
    }
}