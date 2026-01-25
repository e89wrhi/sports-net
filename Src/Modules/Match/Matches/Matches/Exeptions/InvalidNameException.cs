using Sport.Common.BaseExceptions;

namespace Match.Exeptions;

public class InvalidNameException : DomainException
{
    public InvalidNameException(string name)
        : base($"name: '{name}' is invalid.")
    {
    }
}