using Match.Features.CreateMatch.V1;
using Match.Features.DeleteMatch.V1;
using Match.Features.UpdateMatch.V1;
using Match.Features.UpdateMatchStat.V1;
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
            MatchCreatedDomainEvent e => new CreateMatchMongo(e.Id, e.HomeTeam, e.AwayTeam,
             e.League.ToString(), e.Status.ToString(), e.MatchTime),
            MatchUpdatedDomainEvent e => new UpdateMatchMongo(e.Id, e.HomeTeam, e.AwayTeam,
             e.League.ToString(), e.Status.ToString(), e.MatchTime),
            MatchDeletedDomainEvent e => new DeleteMatchMongo(e.Id),
            MatchScoreUpdatedDomainEvent e => new UpdateMatchStatMongo(e.Id, e.HomeTeamScore, e.AwayTeamScore),
            _ => null
        };
    }
}