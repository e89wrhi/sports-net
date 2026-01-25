using Sport.Common.BaseExceptions;

namespace Events.Exeptions;

public class InvalidTimeException : DomainException
{
    public InvalidTimeException(DateTime time)
        : base($"time: '{time}' is invalid.")
    {
    }
}