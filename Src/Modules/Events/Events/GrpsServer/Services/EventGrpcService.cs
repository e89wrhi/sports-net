using System.Threading.Tasks;
using Grpc.Core;
using Mapster;
using MediatR;
using @event;
using Event.Events.Features.GetEventById.V1;

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

    public override async Task<GetEventByIdResult> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetEventById(Guid.Parse(request.Id)));

        return new GetEventByIdResult
        {
            EventDto = new EventResponse
            {
                Id = result.EventDto.Id.ToString(),
                MatchId = result.EventDto.MatchId.ToString(),
                Title = result.EventDto.Title,
                Time = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.EventDto.Time.ToUniversalTime()),
                Type = result.EventDto.Type.ToString()
            }
        };
    }
}