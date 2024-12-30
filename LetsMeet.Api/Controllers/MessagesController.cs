using LetsMeet.Application.Message.Commands.SendMessage;
using LetsMeet.Application.Message.Queries;
using LetsMeet.Application.Room.Commands.CreateRoom;
using LetsMeet.Application.Room.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [EndpointSummary("Send message")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
    {
        var result = await sender.Send(command);
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get messages from room")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMessages([FromQuery] string roomId)
    {
        var result = await sender.Send(new GetMessagesFromRoomQuery(roomId));
        return Ok(result);
    }
}