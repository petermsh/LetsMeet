using FluentValidation;

namespace LetsMeet.Application.User.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Bio)
            .MinimumLength(5)
            .MaximumLength(1000);

        RuleFor(x => x.City)
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