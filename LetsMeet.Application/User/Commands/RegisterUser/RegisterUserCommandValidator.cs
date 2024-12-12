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
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.University)
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Major)
            .MinimumLength(3)
            .MaximumLength(50);
    }
}