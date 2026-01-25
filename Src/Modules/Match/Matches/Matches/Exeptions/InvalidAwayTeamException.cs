using Sport.Common.BaseExceptions;

namespace Match.Exeptions;

public class InvalidAwayTeamException : DomainException
{
    public InvalidAwayTeamException(string team)
        : base($"away team: '{team}' is invalid.")
    {
    }
}