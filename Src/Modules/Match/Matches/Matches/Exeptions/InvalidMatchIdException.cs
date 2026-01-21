using Sport.Common.BaseExceptions;
namespace Sport.Matchs.Exeptions;

public class InvalidMatchIdException : DomainException
{
    public InvalidMatchIdException(Guid matchId)
        : base($"matchId: '{matchId}' is invalid.")
    {
    }
}