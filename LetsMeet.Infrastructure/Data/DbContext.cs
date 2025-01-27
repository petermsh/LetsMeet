using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Domain.Common;
using LetsMeet.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Infrastructure.Data;

internal class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options), IDataContext
{
    //public DbSet<Entity> Entities => Set<Entity>();

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AppUser>()
            .HasMany(x => x.Rooms)
            .WithMany(x => x.Users);
        
        builder.HasDefaultSchema("LetsMeet");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}