using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Application.User.Commands.ChangeStatus;

public record ChangeStatusCommand(AppUser user, bool status): IRequest;

public class ChangeStatusCommandHandler(IDataContext context, UserManager<AppUser> userManager) : IRequestHandler<ChangeStatusCommand>
{
    public async Task Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
    {
        request.user.Status = request.status;

        var result = await userManager.UpdateAsync(request.user);
        if (!result.Succeeded)
        {
            throw new ErrorsOccuredException("Problem with status changing");
        }
    }
}