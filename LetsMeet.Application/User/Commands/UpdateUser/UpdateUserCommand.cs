using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.User.Commands.UpdateUser;

public record UpdateUserCommand(string? Bio, string? City, string? University, string? Major) : IRequest;

public class UpdateUserCommandHandler(ICurrentUser currentUser, IDataContext context) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.Id ?? throw new UserNotFoundException("");

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken) 
                   ?? throw new UserNotFoundException("");

        if(!string.IsNullOrEmpty(request.Bio))
            user.Bio = request.Bio;

        if(!string.IsNullOrEmpty(request.City))
            user.City = request.City;

        if(!string.IsNullOrEmpty(request.University))
            user.University = request.University;

        if(!string.IsNullOrEmpty(request.Major))
            user.Major = request.Major;

        await context.SaveChangesAsync(cancellationToken);
    }
}