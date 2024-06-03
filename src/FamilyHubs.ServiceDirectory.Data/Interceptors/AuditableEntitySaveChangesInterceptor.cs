using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using FamilyHubs.SharedKernel.Identity;

namespace FamilyHubs.ServiceDirectory.Data.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditableEntitySaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context is null) return;

        long? updatedBy = null;
        var user = _httpContextAccessor?.HttpContext?.GetFamilyHubsUser();
        if (user != null && long.TryParse(user.AccountId, out var newUpdatedBy))
        {
            updatedBy = newUpdatedBy;
        }

        //todo: is this needed?
        foreach (var entry in context.ChangeTracker.Entries<ServiceLocationSharedEntityBase>()
                     .Where(e => e is { State: EntityState.Modified, Entity: { ServiceId: null, LocationId: null } }))
        {
            entry.State = EntityState.Deleted;
        }

        var now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<EntityBase<long>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = updatedBy;
                entry.Entity.Created = now;
            }

            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = updatedBy;
                entry.Entity.LastModified = now;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}