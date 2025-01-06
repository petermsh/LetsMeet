using LetsMeet.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Room.Queries.IsRoomExistingQuery;

public record IsRoomExistingQuery(string RoomId): IRequest<bool>;

public class IsRoomExistingQueryHandler(IDataContext context) : IRequestHandler<IsRoomExistingQuery, bool>
{
    public async Task<bool> Handle(IsRoomExistingQuery request, CancellationToken cancellationToken)
    {
        return await context.Rooms.AnyAsync(x => x.Id == request.RoomId, cancellationToken);
    }
}

