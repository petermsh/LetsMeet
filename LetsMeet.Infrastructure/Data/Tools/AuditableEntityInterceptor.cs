using LetsMeet.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LetsMeet.Infrastructure.Data.Tools;

internal class AuditableEntityInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = timeProvider.GetUtcNow();

        var auditableEntities = context.ChangeTracker.Entries()
            .Where(x => x is { Entity: IAuditable, State: EntityState.Added or EntityState.Modified });

        foreach (var entry in auditableEntities)
        {
            if (entry.Entity is not IAuditable auditableEntity) 
                continue;

            if (entry.State == EntityState.Added)
            {
                auditableEntity.CreatedAt = utcNow;
            }

            auditableEntity.ModifiedAt = utcNow;
        }
    }
}