
using System.Net;

namespace Sport.Common.Exception;

/// <summary>
/// Thrown when the client sends a request that is syntactically incorrect or violates simple business rules.
/// </summary>
public class BadRequestException : CustomException
{
    public BadRequestException(string message, int? code = null) : base(message, HttpStatusCode.BadRequest, code: code)
    {

    }
}