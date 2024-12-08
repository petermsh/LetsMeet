using LetsMeet.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LetsMeet.Infrastructure.Data.Tools;

internal class AuditableEntityInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = timeProvider.GetUtcNow();

        var auditableEntities = context.ChangeTracker.Entries()
            .Where(x => x is { Entity: IAuditable, State: EntityState.Added or EntityState.Modified })
            .ToList();

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