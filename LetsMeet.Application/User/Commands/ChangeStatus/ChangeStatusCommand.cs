using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.User.Commands.ChangeStatus;

public record ChangeStatusCommand(bool Status): IRequest;

public class ChangeStatusCommandHandler(ICurrentUser currentUser, IDataContext context, UserManager<AppUser> userManager) : IRequestHandler<ChangeStatusCommand>
{
    public async Task Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var id = currentUser.Id ?? throw new UserNotFoundException("");

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken)
                   ?? throw new UserNotFoundException(id.ToString());
        user.Status = request.Status;
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new ErrorsOccuredException("Problem with status changing");
        }
    }
}