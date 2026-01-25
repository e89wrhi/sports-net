using Events.Models;
using Sport.Common.Contracts.EventBus.Messages;
using Sport.Common.Core;

namespace Events.Features;

public class EventsMappings : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            EventCreatedDomainEvent e => new EventCreatedIntegrationEvent(e.Id, e.MatchId, e.Title, e.Time, (int)e.Type),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event) => null;
}
