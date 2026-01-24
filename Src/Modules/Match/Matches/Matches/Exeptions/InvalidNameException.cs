using Sport.Common.BaseExceptions;

namespace Match.Matches.Exeptions;

public class InvalidNameException : DomainException
{
    public InvalidNameException(string name)
        : base($"name: '{name}' is invalid.")
    {
    }
}