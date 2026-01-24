using Sport.Common.BaseExceptions;

namespace Vote.Votes.Exeptions;

public class InvalidVoterIdException : DomainException
{
    public InvalidVoterIdException(Guid voterId)
        : base($"voterId: '{voterId}' is invalid.")
    {
    }
}