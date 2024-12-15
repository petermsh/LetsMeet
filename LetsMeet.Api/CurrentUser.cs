using System.Security.Claims;
using LetsMeet.Application.Common.Interfaces;

namespace LetsMeet;

internal class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private string? _id => httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
    public Guid? Id => Guid.TryParse(_id, out Guid userId) ? userId : null;
    public string? UserName => httpContextAccessor.HttpContext?.User?.FindFirstValue("username");
}