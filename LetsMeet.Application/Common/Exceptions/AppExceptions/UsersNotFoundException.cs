using System.Net;

namespace LetsMeet.Application.Common.Exceptions.AppExceptions;

public class UsersNotFoundException() : AppException($"Users not found.", HttpStatusCode.NotFound)
{
    public override string Type => "users-not-found";
}