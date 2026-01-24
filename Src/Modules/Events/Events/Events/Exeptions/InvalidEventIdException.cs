using Sport.Common.BaseExceptions;

namespace Sport.Events.Exeptions;

public class InvalidEventIdException : DomainException
{
    public InvalidEventIdException(Guid eventId)
        : base($"eventId: '{eventId}' is invalid.")
    {
    }
}