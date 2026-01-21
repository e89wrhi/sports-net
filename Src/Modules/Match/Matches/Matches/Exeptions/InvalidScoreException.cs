using Sport.Common.BaseExceptions;

namespace Matches.Matches.Exeptions;

public class InvalidScoreException : DomainException
{
    public InvalidScoreException(int score)
        : base($"score: '{score}' is invalid.")
    {
    }
}