using LetsMeet.Domain.Common;
using LetsMeet.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Domain.Entities;

public class AppUser : IdentityUser<Guid>, IAuditable
{
    public required int Age { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public required string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
}