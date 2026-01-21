using Matches.Matches.Exeptions;

namespace Matches.Matches.ValueObjects;

public record Score
{
    public int Value { get; }

    private Score(int value)
    {
        Value = value;
    }

    public static Score Of(int value)
    {
        if (value < 0)
        {
            throw new InvalidScoreException(value);
        }

        return new Score(value);
    }

    public static implicit operator int(Score score)
    {
        return score.Value;
    }
}