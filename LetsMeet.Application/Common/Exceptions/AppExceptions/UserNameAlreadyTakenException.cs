namespace LetsMeet.Application.Common.Exceptions.AppExceptions;

public class UserNameAlreadyTakenException(string userName) : AppException($"UserName {userName} is already in use.")
{
    public override string Type => "username-in-use";
}