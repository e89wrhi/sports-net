using Sport.Common.BaseExceptions;

namespace Matches.Matches.Exeptions;

public class InvalidHomeTeamException : DomainException
{
    public InvalidHomeTeamException(string team)
        : base($"home team: '{team}' is invalid.")
    {
    }
}