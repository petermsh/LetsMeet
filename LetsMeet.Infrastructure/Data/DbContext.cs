using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Infrastructure.Data;

internal class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options), IDataContext
{
    //public DbSet<Entity> Entities => Set<Entity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("LetsMeet");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}