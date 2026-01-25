using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using @event;
using Events.Features.GetEvents.V1;

namespace Event.GrpcServer.Services;

public class EventGrpcServices : EventGrpcService.EventGrpcServiceBase
{
    private readonly IMediator _mediator;

    public EventGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetEventsResult> GetEvents(GetEventsRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetEvents { MatchId = Guid.Parse(request.MatchId) });

        var response = new GetEventsResult();
        foreach (var eventDto in result.EventDtos)
        {
            response.EventDtos.Add(new EventResponse
            {
                Id = eventDto.Id.ToString(),
                MatchId = eventDto.MatchId.ToString(),
                Title = eventDto.Title,
                Time = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(eventDto.Time.ToUniversalTime()),
                Type = eventDto.Type.ToString()
            });
        }

        return response;
    }
}