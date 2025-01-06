using System.Net;
using LetsMeet.Application.Common.Exceptions;

namespace LetsMeet.Application.Room.Exceptions;

public class RoomNotFoundException(string roomId) : AppException($"Room {roomId} not found.", HttpStatusCode.NotFound)
{
    public override string Type => "room-not-found";
}