using Sport.Common.BaseExceptions;

namespace Events.Exeptions;

public class InvalidTitleException : DomainException
{
    public InvalidTitleException(string title)
        : base($"title: '{title}' is invalid.")
    {
    }
}