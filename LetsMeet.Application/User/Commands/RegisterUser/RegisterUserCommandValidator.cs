using FluentValidation;
using LetsMeet.Application.Common;

namespace LetsMeet.Application.User.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .Matches(ValidationRules.UserNameRule);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(ValidationRules.PasswordRule);

        RuleFor(x => x.Gender).IsInEnum();

        RuleFor(x => x.Age)
            .NotEmpty()
            .GreaterThanOrEqualTo(18)
            .LessThanOrEqualTo(120);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.University)
            .MaximumLength(50);

        RuleFor(x => x.Major)
            .MaximumLength(50);
    }
}