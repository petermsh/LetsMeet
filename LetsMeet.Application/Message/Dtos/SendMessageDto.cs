namespace LetsMeet.Application.Message.Dtos;

public class SendMessageDto
{
    public string From { get; set; }
    public string Content { get; set; }
    public string Date { get; set; }
    public string RoomId { get; set; }
}