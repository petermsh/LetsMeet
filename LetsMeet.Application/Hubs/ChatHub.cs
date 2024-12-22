using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.User.Commands.ChangeStatus;
using LetsMeet.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Hubs;

//[Authorize]
public class ChatHub(UserManager<AppUser> userManager, ISender sender) : Hub
{
    
    public override async Task OnConnectedAsync()
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == Context.User.Identity.Name);

        if (user is null)
            throw new UserNotFoundException("");

        await sender.Send(new ChangeStatusCommand(user, true));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == Context.User.Identity.Name);

        if (user is null)
            throw new UserNotFoundException("");

        await sender.Send(new ChangeStatusCommand(user, false));
    }
}