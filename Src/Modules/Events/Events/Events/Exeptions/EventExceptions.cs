using Sport.Common.BaseExceptions;

namespace Sport.Events.Exceptions;

public class EventNotFoundException : NotFoundException
{
    public EventNotFoundException(Guid id) : base($"Event with id '{id}' was not found.")
    {
    }
}

public class EventAlreadyExistException : BadRequestException
{
    public EventAlreadyExistException(Guid id) : base($"Event with id '{id}' already exists.")
    {
    }
}
