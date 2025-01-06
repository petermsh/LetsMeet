using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Application.Hubs.Dto;
using LetsMeet.Application.Message.Commands.SendMessage;
using LetsMeet.Application.User.Commands.ChangeStatus;
using LetsMeet.Domain.Entities;
using LetsMeet.Application.Room.Exceptions;
using LetsMeet.Application.Room.Queries.IsRoomExistingQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Hubs;

[Authorize]
public class ChatHub(UserManager<AppUser> userManager, ISender sender, ICurrentUser currentUser) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == currentUser.UserName) 
                   ?? throw new UserNotFoundException("");

        await sender.Send(new ChangeStatusCommand(user, true));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == currentUser.UserName) 
                   ?? throw new UserNotFoundException("");

        await sender.Send(new ChangeStatusCommand(user, false));
    }
    
    public async Task JoinRoom(JoinRoomDto roomDto)
    {
        Console.WriteLine($"JoinRoom called with roomId: {roomDto.RoomId}");
        var room = await sender.Send(new IsRoomExistingQuery(roomDto.RoomId));

        if (!room)
            throw new RoomNotFoundException(roomDto.RoomId);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomDto.RoomId);
        await Clients.Group(roomDto.RoomId).SendAsync("ReceiveMessage", $"{currentUser.UserName} has joined the room.");
    }

    public async Task LeaveRoom(string roomId)
    {
        var room = await sender.Send(new IsRoomExistingQuery(roomId));

        if (!room)
            throw new RoomNotFoundException(roomId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveMessage", $"{currentUser.UserName} has left the room.");
    }

    public async Task SendMessage(SendMessageCommand sendMessage)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == currentUser.UserName) 
                   ?? throw new UserNotFoundException("");
        
        var result = await sender.Send(new SendMessageCommand(sendMessage.RoomId, sendMessage.Content));
    
        await Clients.Group(sendMessage.RoomId).SendAsync("ReceiveMessage", result.Message.Content);
    }
    
}