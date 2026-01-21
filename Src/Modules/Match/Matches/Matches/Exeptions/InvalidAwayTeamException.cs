using Sport.Common.BaseExceptions;

namespace Matches.Matches.Exeptions;

public class InvalidAwayTeamException : DomainException
{
    public InvalidAwayTeamException(string team)
        : base($"away team: '{team}' is invalid.")
    {
    }
}