using LetsMeet.Domain.Enums;

namespace LetsMeet.Application.User.Queries.GetUserInfo;

public record AppUserDto
{
    public Guid Id { get; init; }
    public string UserName { get; init; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
}