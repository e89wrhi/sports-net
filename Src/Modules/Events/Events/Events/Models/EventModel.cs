using Events.Events.Features.AddEvent.V1;
using Events.Events.ValueObjects;
using Sport.Common.Core;
namespace Events.Events.Models;

public record EventModel : Aggregate<EventId>
{
    public MatchId MatchId { get; private set; } = default!;
    public Title Title { get; private set; } = default!;
    public Time Time { get; private set; } = default!;
    public EventType Type { get; private set; } = default!;

    public static EventModel Create(MatchId matchId, Title title, Time time, EventType type)
    {
        var item = new EventModel()
        {
            MatchId = matchId,
            Title = title,
            Time = time,
            Type = type
        };

        var @event = new EventCreatedDomainEvent();
        item.AddDomainEvent(@event);
        return item;
    }
}
