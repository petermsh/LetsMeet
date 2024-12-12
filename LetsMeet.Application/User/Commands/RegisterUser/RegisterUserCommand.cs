using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Domain.Entities;
using LetsMeet.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Application.User.Commands.RegisterUser;

public record RegisterUserCommand(string UserName, string Password, int Age, Gender Gender,
    string City, string? University, string? Major) : IRequest;

public class RegisterUserCommandHandler(IDataContext context, UserManager<AppUser> userManager)
    : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Age = request.Age,
            Gender = request.Gender,
            City = request.City,
            University = request.University,
            Major = request.Major
        };

        if (context.Users.Any(u => u.NormalizedUserName == user.UserName.ToUpper()))
        {
            throw new UserNameAlreadyTakenException(request.UserName);
        }

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new ErrorsOccuredException("Problem with creating an user");
        }
    }
}