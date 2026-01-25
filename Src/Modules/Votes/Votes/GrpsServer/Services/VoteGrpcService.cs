using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using vote;
using Vote.Votes.Features.AddVote.V1;
using Vote.Votes.Enums;

namespace Vote.GrpcServer.Services;

public class VoteGrpcServices : VoteGrpcService.VoteGrpcServiceBase
{
    private readonly IMediator _mediator;

    public VoteGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<AddVoteResponse> AddVote(AddVoteRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new AddVoteCommand(
            Guid.Parse(request.MatchId),
            Guid.Parse(request.VoterId),
            (VoteType)request.Type));

        return new AddVoteResponse
        {
            Id = result.Id.ToString()
        };
    }
}