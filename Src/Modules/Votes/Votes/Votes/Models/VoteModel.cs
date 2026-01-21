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
            VoterId = voterId,
            MatchId = matchId,
            Type = type,
            VotedAt = votedAt
        };

        var @event = new VoteCreatedDomainEvent();
        item.AddDomainEvent(@event);
        return item;
    }
}
