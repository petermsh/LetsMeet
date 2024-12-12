namespace LetsMeet.Application.Common.Exceptions.AppExceptions;

public class ErrorsOccuredException(string message = "") : AppException(message)
{
    public override string Type => "errors-occured";
}