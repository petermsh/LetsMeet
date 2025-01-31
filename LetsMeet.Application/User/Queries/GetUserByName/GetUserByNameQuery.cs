using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Application.User.Queries.GetUserInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.User.Queries.GetUserByName;

public record GetUserByNameQuery(string UserName) : IRequest<AppUserDto>;

public class GetUserByNameQueryHandler(IDataContext context) : IRequestHandler<GetUserByNameQuery, AppUserDto>
{
    public async Task<AppUserDto> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken: cancellationToken)
                   ?? throw new UserNotFoundException(request.UserName);

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