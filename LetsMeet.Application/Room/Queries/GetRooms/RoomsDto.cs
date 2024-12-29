namespace LetsMeet.Application.Room.Queries.GetRooms;

public record RoomsDto
{
    public string RoomId { get; init; }
    public string? RoomName { get; init; }
    public string? LastMessage { get; init; }
}