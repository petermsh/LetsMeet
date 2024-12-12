namespace LetsMeet.Application.Common;

internal static class ValidationRules
{
    public const string PasswordRule = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[A-Za-z\d!@#$%^&*()_+=-])[A-Za-z\d!@#$%^&*()_+=-]{8,200}$";
    public const string UserNameRule = @"^[a-zA-Z0-9]*$";
}