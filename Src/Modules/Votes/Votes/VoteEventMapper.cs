using Sport.Common.Contracts.EventBus.Messages;
using Sport.Common.Core;
using Vote.Features.AddVote.V1;
using Vote.Features.DeleteVote.V1;
using Vote.Models;

namespace Vote;


// ref: https://www.ledjonbehluli.com/posts/domain_to_integration_event/
public sealed class VoteEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            VoteCreatedDomainEvent e => new VoteCreated(e.Id),
            VoteDeletedDomainEvent e => new VoteDeleted(e.Id),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            VoteCreatedDomainEvent e => new AddVoteMongo(e.Id, e.MatchId, e.VoterId, e.Type.ToString()),
            VoteDeletedDomainEvent e => new DeleteVoteMongo(e.Id, e.MatchId, e.VoterId),
            _ => null
        };
    }
}