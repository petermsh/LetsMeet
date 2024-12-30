using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Application.Message.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Message.Queries;

public record GetMessagesFromRoomQuery(string RoomId) : IRequest<List<MessagesDto>>;

public class GetMessagesFromRoomQueryHandler(ICurrentUser currentUser, IDataContext context) : IRequestHandler<GetMessagesFromRoomQuery, List<MessagesDto>>
{
    public async Task<List<MessagesDto>> Handle(GetMessagesFromRoomQuery request, CancellationToken cancellationToken)
    {
        return await context.Messages.Where(x => x.RoomId == request.RoomId)
            .Select(q => new MessagesDto()
            {
                Id = q.Id,
                From = q.SenderUserName,
                Content = q.Content,
                Date = q.CreatedAt,
                IsFromUser = currentUser.UserName == q.SenderUserName
            }).OrderBy(m => m.Date).ToListAsync(cancellationToken);
    }
}