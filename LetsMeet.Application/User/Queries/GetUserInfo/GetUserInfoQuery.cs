using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.User.Queries.GetUserInfo;

public record GetUserInfoQuery : IRequest<AppUserDto>;

public class GetUserInfoQueryHandler(ICurrentUser currentUser, IDataContext context) : IRequestHandler<GetUserInfoQuery, AppUserDto>
{
    public async Task<AppUserDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var id = currentUser.Id ?? throw new UserNotFoundException("");

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken)
                   ?? throw new UserNotFoundException(id.ToString());

        var userDto = new AppUserDto
        {
            Id = user.Id,
            UserName = user.UserName!,
            Age = user.Age,
            Gender = user.Gender,
            Bio = user.Bio,
            City = user.City,
            University = user.University,
            Major = user.Major
        };

        return userDto;
    }
}