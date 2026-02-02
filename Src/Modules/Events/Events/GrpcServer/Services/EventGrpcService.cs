using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Event;
using Events.Features.GetEvents.V1;
using Events.Dtos;
using Google.Protobuf.WellKnownTypes;

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
        foreach (var dto in result.EventDtos)
        {
            response.EventDtos.Add(new EventResponse
            {
                Id = dto.Id.ToString(),
                MatchId = dto.MatchId.ToString(),
                Title = dto.Title,
                Time = Timestamp.FromDateTime(dto.Time.ToUniversalTime()),
                Type = dto.Type.ToString()
            });
        }

        return response;
    }
}