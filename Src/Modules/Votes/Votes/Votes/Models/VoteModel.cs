using Sport.Common.Core;
using Votes.Votes.Enums;
using Votes.Votes.ValueObjects;

namespace Votes.Votes.Models;

public record VoteModel : Aggregate<VoteId>
{
    public VoterId VoterId { get; private set; } = default!;
    public MatchId MatchId { get; private set; } = default!;
    public VoteType Type { get; private set; } = default!;
    public DateTime VotedAt { get; private set; } = default!;

    public static VoteModel Create(VoterId voterId, MatchId matchId,
        VoteType type, DateTime votedAt)
    {
        var item = new VoteModel()
        {
            Id = VoteId.Of(Guid.NewGuid()),
            VoterId = voterId,
            MatchId = matchId,
            Type = type,
            VotedAt = votedAt,
            Version = 1
        };

        var @event = new VoteCreatedDomainEvent(item.Id, item.MatchId, item.VoterId, item.Type);
        item.AddDomainEvent(@event);
        return item;
    }
}

public record VoteCreatedDomainEvent(VoteId Id, MatchId MatchId, VoterId VoterId, VoteType Type) : IDomainEvent;
public record VoteDeletedDomainEvent(VoteId Id, MatchId MatchId, VoterId VoterId, VoteType Type): IDomainEvent;