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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AppUser>()
            .HasMany(x => x.Rooms)
            .WithMany(x => x.Users);
        
        builder.HasDefaultSchema("LetsMeet");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            var entity = entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTimeOffset.UtcNow;
                entity.ModifiedAt = DateTimeOffset.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.ModifiedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}