using Sport.Common.BaseExceptions;

namespace Sport.Matchs.Exceptions;

public class MatchNotFoundException : NotFoundException
{
    public MatchNotFoundException(Guid id) : base($"Match with id '{id}' was not found.")
    {
    }
}

public class MatchAlreadyExistException : BadRequestException
{
    public MatchAlreadyExistException(Guid id) : base($"Match with id '{id}' already exists.")
    {
    }
}
