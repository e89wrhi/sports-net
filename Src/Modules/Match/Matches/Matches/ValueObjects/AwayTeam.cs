using Match.Matches.Exeptions;

namespace Match.Matches.ValueObjects;

public record AwayTeam
{
    public string Value { get; }

    private AwayTeam(string value)
    {
        Value = value;
    }

    public static AwayTeam Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidAwayTeamException(value);
        }

        return new AwayTeam(value);
    }

    public static implicit operator string(AwayTeam team)
    {
        return team.Value;
    }
}