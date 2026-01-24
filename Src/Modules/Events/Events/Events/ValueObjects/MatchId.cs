using Sport.Events.Exeptions;

namespace Event.Events.ValueObjects;

public record MatchId
{
    public Guid Value { get; }

    private MatchId(Guid value)
    {
        Value = value;
    }

    public static MatchId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidMatchIdException(value);
        }

        return new MatchId(value);
    }

    public static implicit operator Guid(MatchId id)
    {
        return id.Value;
    }
}