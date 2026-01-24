using Sport.Votes.Exeptions;

namespace Votes.Votes.ValueObjects;

public record VoteId
{
    public Guid Value { get; }

    private VoteId(Guid value)
    {
        Value = value;
    }

    public static VoteId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidVoteIdException(value);
        }

        return new VoteId(value);
    }

    public static implicit operator Guid(VoteId id)
    {
        return id.Value;
    }
} 