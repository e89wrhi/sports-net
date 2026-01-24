using Sport.Common.Contracts.EventBus.Messages;
using Sport.Common.Core;
using Vote.Votes.Models;

namespace Vote.Votes.Features;

public class VotesMappings : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            VoteCreatedDomainEvent e => new VoteCreatedIntegrationEvent(e.Id, e.MatchId, (int)e.Type),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event) => null;
}
