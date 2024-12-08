using LetsMeet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Common;

public interface IDataContext
{
    DbSet<User> Users { get; }
}