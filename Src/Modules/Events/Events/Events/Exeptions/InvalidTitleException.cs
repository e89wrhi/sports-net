using Sport.Common.BaseExceptions;

namespace Event.Events.Exeptions;

public class InvalidTitleException : DomainException
{
    public InvalidTitleException(string title)
        : base($"title: '{title}' is invalid.")
    {
    }
}