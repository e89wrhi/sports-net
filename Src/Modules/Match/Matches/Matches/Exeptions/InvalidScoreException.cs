using Sport.Common.BaseExceptions;

namespace Match.Exeptions;

public class InvalidScoreException : DomainException
{
    public InvalidScoreException(int score)
        : base($"score: '{score}' is invalid.")
    {
    }
}