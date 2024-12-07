using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LetsMeet.Application.Common;

namespace LetsMeet.Infrastructure.Data;

internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IDataContext
{
    //public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("LetsMeet");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}