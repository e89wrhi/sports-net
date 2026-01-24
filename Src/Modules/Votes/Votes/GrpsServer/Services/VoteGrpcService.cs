using System.Threading.Tasks;
using Grpc.Core;
using Mapster;
using MediatR;

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
}