using System.Threading.Tasks;
using Grpc.Core;
using Mapster;
using MediatR;

namespace Event.GrpcServer.Services;

using Grpc.Core;
using System;

public class EventGrpcServices : EventGrpcService.EventGrpcServiceBase
{
    private readonly IMediator _mediator;

    public EventGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }
}