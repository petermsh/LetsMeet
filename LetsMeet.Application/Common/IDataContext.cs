using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Common;

public interface IDataContext
{
    DbSet<Domain.Entities.AppUser> Users { get; }
}