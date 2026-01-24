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
            Id = EventId.Of(Guid.NewGuid()),
            MatchId = matchId,
            Title = title,
            Time = time,
            Type = type,
            Version = 1
        };

        var @event = new EventCreatedDomainEvent(item.Id, item.MatchId, item.Title, item.Time, item.Type);
        item.AddDomainEvent(@event);
        return item;
    }
}

public record EventCreatedDomainEvent(EventId Id, MatchId MatchId, Title Title, Time Time, EventType Type) : IDomainEvent;

public record EventDeletedDomainEvent(EventId Id, MatchId MatchId) : IDomainEvent;