using Event.Events.Models;
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
            EventCreatedDomainEvent e => new CreateEventMongo(),
            EventDeletedDomainEvent e => new DeleteEventMongo(),
            _ => null
        };
    }
}