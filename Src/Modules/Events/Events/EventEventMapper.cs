using Events.Features.AddEvent.V1;
using Events.Features.DeleteEvent.V1;
using Events.Models;
using Sport.Common.Contracts.EventBus.Messages;
using Sport.Common.Core;

namespace Event;


// ref: https://www.ledjonbehluli.com/posts/domain_to_integration_event/
public sealed class EventEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            EventCreatedDomainEvent e => new EventCreated(e.Id),
            EventDeletedDomainEvent e => new EventDeleted(e.Id),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            EventCreatedDomainEvent e => new AddEventMongo(e.Id, e.MatchId, e.Title, e.Time,
            e.Type.ToString()),
            EventDeletedDomainEvent e => new DeleteEventMongo(e.Id),
            _ => null
        };
    }
}