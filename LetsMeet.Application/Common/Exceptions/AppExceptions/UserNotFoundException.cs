using System.Net;

namespace LetsMeet.Application.Common.Exceptions.AppExceptions;

public class UserNotFoundException(string user) : AppException($"User {user} not found.", HttpStatusCode.NotFound)
{
    public override string Type => "user-not-found";
}