using System.Threading.Tasks;
using Grpc.Core;
using Mapster;
using MediatR;

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

}