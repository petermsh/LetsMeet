using LetsMeet.Application.Common.Exceptions.AppExceptions;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Application.Hubs;
using LetsMeet.Application.Message.Dtos;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Message.Commands.SendMessage;

public record SendMessageCommand(string RoomId, string Content): IRequest<SendMessageCommand.Result>
{
    public record Result(SendMessageDto Message);
}

public class SendMessageHandler(IDataContext context, ICurrentUser currentUser, IHubContext<ChatHub> hubContext) : IRequestHandler<SendMessageCommand, SendMessageCommand.Result>
{
    public async Task<SendMessageCommand.Result> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == currentUser.Id,
                       cancellationToken)
                   ?? throw new UserNotFoundException("");

        var message = new Domain.Entities.Message()
        {
            Content = request.Content,
            RoomId = request.RoomId,
            SenderUserName = user.UserName
        };
        
        var sendMessage = new SendMessageDto
        {
            Content = message.Content,
            Date = message.MessageSent,
            From = message.SenderUserName,
            RoomId = message.RoomId
        };
        
        //await hubContext.Clients.Group(sendMessage.RoomId).SendAsync("ReceiveMessage", sendMessage);
        await context.Messages.AddAsync(message, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new SendMessageCommand.Result(sendMessage);
    }
}