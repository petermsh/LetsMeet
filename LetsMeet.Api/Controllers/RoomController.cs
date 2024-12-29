using LetsMeet.Application.Room.Commands.CreateRoom;
using LetsMeet.Application.Room.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [EndpointSummary("Create room")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        await sender.Send(command);
        return NoContent();
    }
    
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get rooms")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRooms()
    {
        var result = await sender.Send(new GetRoomsQuery());
        return Ok(result);
    }
}