using LetsMeet.Application.User.Commands.ChangeStatus;
using LetsMeet.Application.User.Commands.UpdateUser;
using LetsMeet.Application.User.Queries.GetUserByName;
using LetsMeet.Application.User.Queries.GetUserInfo;
using LetsMeet.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get user info")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserInfo()
    {
        var result = await sender.Send(new GetUserInfoQuery());
        return Ok(result);
    }
    
    [HttpGet("{userName}")]
    [Authorize]
    [EndpointSummary("Get user info by name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByName(string userName)
    {
        var result = await sender.Send(new GetUserByNameQuery(userName));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize]
    [EndpointSummary("Change user status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeStatus(bool status)
    {
        await sender.Send(new ChangeStatusCommand(status));
        return NoContent();
    }

    [HttpPatch("info")]
    [Authorize]
    [EndpointSummary("Update user info")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        await sender.Send(command);
        return NoContent();
    }
}