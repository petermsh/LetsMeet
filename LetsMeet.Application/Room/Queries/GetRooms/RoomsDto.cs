namespace LetsMeet.Application.Room.Queries.GetRooms;

public record RoomsDto
{
    public string RoomId { get; init; }
    public string RoomName { get; init; }
    public LastMessageDto LastMessage { get; init; }
}

public record LastMessageDto
{
    public string Content { get; init; }
    public string Date { get; init; }
}