using Sport.Common.BaseExceptions;

namespace Sport.Votes.Exceptions;

public class VoteNotFoundException : NotFoundException
{
    public VoteNotFoundException(Guid id) : base($"Vote with id '{id}' was not found.")
    {
    }
}

public class VoteAlreadyExistException : BadRequestException
{
    public VoteAlreadyExistException(Guid id) : base($"Vote with id '{id}' already exists.")
    {
    }
}
