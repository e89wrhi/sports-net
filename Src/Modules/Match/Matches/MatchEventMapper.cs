using Match.Models;
using Sport.Common.Contracts.EventBus.Messages;
using Sport.Common.Core;

namespace Match;

// ref: https://www.ledjonbehluli.com/posts/domain_to_integration_event/
public sealed class MatchEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            // map to integration event here(if needed)
            MatchCreatedDomainEvent e => new MatchCreated(e.Id),
            MatchUpdatedDomainEvent e => new MatchUpdated(e.Id),
            MatchDeletedDomainEvent e => new MatchDeleted(e.Id),
            MatchScoreUpdatedDomainEvent e => new MatchScoreUpdated(e.Id),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            // map domain events to internal commands to handle changes
            // DomainEvent e => new MethodName(e.SessionId.Value),
            _ => null
        };
    }
}