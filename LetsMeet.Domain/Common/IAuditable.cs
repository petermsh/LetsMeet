namespace LetsMeet.Domain.Common;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset ModifiedAt { get; set; }
}