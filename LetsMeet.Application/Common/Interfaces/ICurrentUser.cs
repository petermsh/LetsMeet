namespace LetsMeet.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid? Id { get; }
    string? UserName  { get; }
}