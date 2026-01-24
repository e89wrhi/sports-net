using Events.Events.Enums;
using Events.Events.ValueObjects;
using Sport.Common.Core;

namespace Events.Events.Models;

public record EventModel : Aggregate<EventId>
{
    public MatchId MatchId { get; private set; } = default!;
    public Title Title { get; private set; } = default!;
    public Time Time { get; private set; } = default!;
    public Enums.EventType Type { get; private set; } = default!;

    public static EventModel Create(MatchId matchId, Title title, Time time, Enums.EventType type)
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

public record EventCreatedDomainEvent(EventId Id, MatchId MatchId, Title Title, Time Time, Enums.EventType Type) : IDomainEvent;

public record EventDeletedDomainEvent(EventId Id, MatchId MatchId) : IDomainEvent;