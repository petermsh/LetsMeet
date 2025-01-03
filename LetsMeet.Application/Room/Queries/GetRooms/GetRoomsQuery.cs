﻿using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Room.Queries.GetRooms;

public record GetRoomsQuery : IRequest<List<RoomsDto>>;

public class GetRoomsQueryHandler(ICurrentUser currentUser, IDataContext context)
    : IRequestHandler<GetRoomsQuery, List<RoomsDto>>
{
    public async Task<List<RoomsDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var id = currentUser.Id ?? throw new UserNotFoundException("");

        var rooms = await context.Rooms.AsNoTracking()
            .Include(x => x.Users)
            .Where(x => x.Users.Any(u => u.Id == id))
            .Select(x => new RoomsDto
            {
                RoomId = x.Id,
                RoomName = x.Users.FirstOrDefault(u => u.Id != id).UserName,
                LastMessage = x.Messages.OrderByDescending(x=>x.CreatedAt).FirstOrDefault().Content
            })
            .ToListAsync(cancellationToken);

        return rooms;
    }
}