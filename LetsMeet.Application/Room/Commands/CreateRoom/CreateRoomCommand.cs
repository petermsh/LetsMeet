using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Application.Hubs;
using LetsMeet.Application.Message.Dtos;
using LetsMeet.Domain.Entities;
using LetsMeet.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Room.Commands.CreateRoom;

public record CreateRoomCommand(string ConnectionId, string? University, string? City, string? Major, Gender Gender) : IRequest<CreateRoomCommand.Result>
{
    public record Result(string Id);
}

public class CreateRoomCommandHandler(IDataContext context, ICurrentUser user, IHubContext<ChatHub> hubContext) : IRequestHandler<CreateRoomCommand, CreateRoomCommand.Result>
{
    public async Task<CreateRoomCommand.Result> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var rng = new Random();
        AppUser secondUser = null;
        
        var userName = user.UserName ?? throw new UserNotFoundException("");
        
        var usersQuery = context.Users;
        
        var currentUser =
            await usersQuery.FirstOrDefaultAsync(x => x.NormalizedUserName == userName.ToUpper(),
                cancellationToken)
            ?? throw new UserNotFoundException(userName);

        var filteredUsers = usersQuery.Where(x => x.Id != currentUser.Id && x.Status == true);

        if (request.Gender != 0)
            filteredUsers = filteredUsers.Where(x => x.Gender == request.Gender);

        if (!string.IsNullOrEmpty(request.City))
            filteredUsers = filteredUsers.Where(x => x.City == request.City);

        if (!string.IsNullOrEmpty(request.University))
            filteredUsers = filteredUsers.Where(x => x.University == request.University);

        if (!string.IsNullOrEmpty(request.Major))
            filteredUsers = filteredUsers.Where(x => x.Major == request.Major);

        if (!await filteredUsers.AnyAsync(cancellationToken))
        { 
            throw new UsersNotFoundException();
        }

        var usersList = await filteredUsers.ToListAsync(cancellationToken);
        
        do
        {
            var index = rng.Next(usersList.Count());
            secondUser = usersList[index];

            var existingRoom = await context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(room => 
                        room.Users.Any(u => u.Id == currentUser.Id) &&
                        room.Users.Any(u => u.Id == secondUser.Id), 
                    cancellationToken);
            
            if (existingRoom != null)
            {
                usersList.RemoveAt(index);
                secondUser = null;
            }

        } while (secondUser == null);

        var room = new Domain.Entities.Room
        {
            Users = new List<AppUser> { currentUser, secondUser }
        };

        await context.Rooms.AddAsync(room, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        await hubContext.Groups.AddToGroupAsync(request.ConnectionId, room.Id);
        var message = new Domain.Entities.Message()
        {
            Content ="Utworzono konwersację",
            RoomId = room.Id,
            SenderUserName = "Server"
        };
        await context.Messages.AddAsync(message, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateRoomCommand.Result(room.Id);
    }
}