using Sport.Common.BaseExceptions;

namespace Sport.Votes.Exeptions;

public class InvalidVoteIdException : DomainException
{
    public InvalidVoteIdException(Guid voteId)
        : base($"voteId: '{voteId}' is invalid.")
    {
    }
} 