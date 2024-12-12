using System.Net;

namespace LetsMeet.Application.Common.Exceptions;

public abstract class AppException(
    string message,
    HttpStatusCode errorCode = HttpStatusCode.BadRequest,
    Exception? innerException = null) : Exception(message, innerException)
{
    public HttpStatusCode ErrorCode { get; } = errorCode;
    public abstract string Type { get; }
}