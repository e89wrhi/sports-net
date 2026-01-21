using Matches.Matches.Exeptions;

namespace Matches.Matches.ValueObjects;

public record HomeTeam 
{
    public string Value { get; }

    private HomeTeam(string value)
    {
        Value = value;
    }

    public static HomeTeam Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidHomeTeamException(value);
        }

        return new HomeTeam(value);
    }

    public static implicit operator string(HomeTeam team)
    {
        return team.Value;
    }
}