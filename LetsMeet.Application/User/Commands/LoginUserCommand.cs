using LetsMeet.Application.Common;
using LetsMeet.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.User.Commands;

public record LoginUserCommand(string UserName, string Password) : IRequest<LoginUserCommand.Result>
{
    public record Result(string AccessToken);
}

public class LoginUserCommandHandler(IDataContext context, SignInManager<AppUser> signInManager, ITokenService tokenService) : IRequestHandler<LoginUserCommand, LoginUserCommand.Result>
{
    public async Task<LoginUserCommand.Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == request.UserName.ToUpper(),
                       cancellationToken)
                   ?? throw new Exception("User not found"); //todo

        var loginResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!loginResult.Succeeded)
        {
            throw new Exception("Invalid login data");
        }

        return new LoginUserCommand.Result(tokenService.CreateAccessToken(user));
    }
}