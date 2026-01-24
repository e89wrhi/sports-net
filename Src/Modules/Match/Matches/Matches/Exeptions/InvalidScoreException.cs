using Sport.Common.BaseExceptions;

namespace Match.Matches.Exeptions;

public class InvalidScoreException : DomainException
{
    public InvalidScoreException(int score)
        : base($"score: '{score}' is invalid.")
    {
    }
}