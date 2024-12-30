using System.ComponentModel.DataAnnotations.Schema;
using LetsMeet.Domain.Common;

namespace LetsMeet.Domain.Entities;

public class Room : IAuditable
{
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public string Id { get; set; }
    public bool IsLocked { get; set; }
    public int EntityStatus { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    
    public ICollection<AppUser> Users { get; set; }
    public ICollection<Message> Messages { get; set; }

    public Room()
    {
    }
}