using Sport.Common.BaseExceptions;

namespace Match.Exeptions;

public class InvalidHomeTeamException : DomainException
{
    public InvalidHomeTeamException(string team)
        : base($"home team: '{team}' is invalid.")
    {
    }
}