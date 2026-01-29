using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Event.Data;
using Events.Dtos;
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
using Sport.Events.Exceptions;

namespace Events.Features.GetEvents.V1;

public record GetEvents : IQuery<GetEventsResult>, ICacheRequest
{
    public Guid MatchId { get; set; }
    public string CacheKey => "GetEvents";
    public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddHours(1);
}

public record GetEventsResult(IEnumerable<EventDto> EventDtos);

public record GetEventsResponseDto(IEnumerable<EventDto> EventDtos);

public class GetEventsEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/event/get-events",
                async (IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetEvents(), cancellationToken);

                    var response = result.Adapt<GetEventsResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("GetEvents")
            .WithApiVersionSet(builder.NewApiVersionSet("Event").Build())
            .Produces<GetEventsResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Events")
            .WithDescription("Get Events")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

internal class GetEventsHandler : IQueryHandler<GetEvents, GetEventsResult>
{
    private readonly IMapper _mapper;
    private readonly EventReadDbContext _eventReadDbContext;

    public GetEventsHandler(IMapper mapper, EventReadDbContext eventReadDbContext)
    {
        _mapper = mapper;
        _eventReadDbContext = eventReadDbContext;
    }

    public async Task<GetEventsResult> Handle(GetEvents request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var @event = (await _eventReadDbContext.Event.AsQueryable().ToListAsync(cancellationToken))
            .Where(i => i.MatchId == request.MatchId);

        if (!@event.Any())
        {
            throw new EventNotFoundException(request.MatchId);
        }

        var eventDtos = _mapper.Map<IEnumerable<EventDto>>(@event);

        return new GetEventsResult(eventDtos);
    }
}
