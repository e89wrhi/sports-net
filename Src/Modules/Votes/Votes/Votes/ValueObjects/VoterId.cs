using Vote.Exeptions;

namespace Vote.ValueObjects;

public record VoterId
{
    public Guid Value { get; }

    private VoterId(Guid value)
    {
        Value = value;
    }

    public static VoterId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidVoterIdException(value);
        }

        return new VoterId(value);
    }

    public static implicit operator Guid(VoterId voteId)
    {
        return voteId.Value;
    }
}