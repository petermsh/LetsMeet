using LetsMeet.Application.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        await sender.Send(command);
        return NoContent();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command)
    {
        var result = await sender.Send(command);
        return Ok(result);
    }
}