using Ardalis.GuardClauses;
using Event.Data;
using Events.Dtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Sport.Common.Core;
using Sport.Events.Exceptions;

namespace Events.Features.GetEvents.V1;

internal class GetEventsHandler : IQueryHandler<GetEvents, GetEventsResult>
{
    private readonly IMapper _mapper;
    private readonly EventDbContext _eventDbContext;

    public GetEventsHandler(IMapper mapper, EventDbContext eventDbContext)
    {
        _mapper = mapper;
        _eventDbContext = eventDbContext;
    }

    public async Task<GetEventsResult> Handle(GetEvents request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var @event = (await _eventDbContext.Events.AsQueryable().ToListAsync(cancellationToken))
            .Where(i => i.MatchId == request.MatchId);

        if (!@event.Any())
        {
            throw new EventNotFoundException(request.MatchId);
        }

        var eventDtos = _mapper.Map<IEnumerable<EventDto>>(@event);

        return new GetEventsResult(eventDtos);
    }
}

