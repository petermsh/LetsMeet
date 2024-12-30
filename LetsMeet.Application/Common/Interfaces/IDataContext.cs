using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Common.Interfaces;

public interface IDataContext
{
    DbSet<Domain.Entities.AppUser> Users { get; }
    DbSet<Domain.Entities.Room> Rooms { get; }
    DbSet<Domain.Entities.Message> Messages { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}