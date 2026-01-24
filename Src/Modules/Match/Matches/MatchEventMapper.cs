using Match.Matches.Models;
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
            MatchCreatedDomainEvent e => new CreateMatchMongo(),
            MatchUpdatedDomainEvent e => new UpdateMatchMongo(),
            MatchDeletedDomainEvent e => new DeleteMatchMongo(),
            MatchScoreUpdatedDomainEvent e => new CreateAircraftMongo(),
            _ => null
        };
    }
}