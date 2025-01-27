using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Application.Hubs.Dto;
using LetsMeet.Application.Message.Commands.SendMessage;
using LetsMeet.Application.Message.Dtos;
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

        await sender.Send(new ChangeStatusCommand(true));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == currentUser.UserName) 
                   ?? throw new UserNotFoundException("");

        await sender.Send(new ChangeStatusCommand(false));
    }
    
    public async Task JoinRoom(JoinRoomDto roomDto)
    {
        Console.WriteLine($"JoinRoom called with roomId: {roomDto.RoomId}");
        var room = await sender.Send(new IsRoomExistingQuery(roomDto.RoomId));

        if (!room)
            throw new RoomNotFoundException(roomDto.RoomId);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomDto.RoomId);
        await Clients.Group(roomDto.RoomId).SendAsync("ReceiveMessage", new SendMessageDto()
            {
                Content = $"{currentUser.UserName} has joined the room.",
                RoomId = roomDto.RoomId
            });
    }

    public async Task LeaveRoom(string roomId)
    {
        var room = await sender.Send(new IsRoomExistingQuery(roomId));

        if (!room)
            throw new RoomNotFoundException(roomId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveMessage",
            new SendMessageDto()
            {
                Content = $"{currentUser.UserName} has left the room.",
                RoomId = roomId
            });
    }

    public async Task SendMessage(SendMessageCommand sendMessage)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == currentUser.UserName) 
                   ?? throw new UserNotFoundException("");
        
        var result = await sender.Send(new SendMessageCommand(sendMessage.RoomId, sendMessage.Content));
    
        var messageDto = new SendMessageDto()
        {
            RoomId = result.Message.RoomId,
            Content = result.Message.Content,
            Date = DateTime.UtcNow.ToString("o"),
            From = user.UserName
        };
        
        await Clients.Group(sendMessage.RoomId).SendAsync("ReceiveMessage", messageDto);
    }
    
}