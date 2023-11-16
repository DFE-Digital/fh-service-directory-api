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

        var updatedBy = "System";
        var user = _httpContextAccessor?.HttpContext?.GetFamilyHubsUser();
        if (user != null && !string.IsNullOrEmpty(user.AccountId))
        {
            updatedBy = user.AccountId;
        }

        foreach (var entry in context.ChangeTracker.Entries<ServiceLocationSharedEntityBase>())
        {
            if (entry is { State: EntityState.Modified, Entity: { ServiceId: null, LocationId: null } })
            {
                entry.State = EntityState.Deleted;
            }
        }

        foreach (var entry in context.ChangeTracker.Entries<EntityBase<long>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = updatedBy;
                entry.Entity.Created = DateTime.UtcNow;
            }

            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = updatedBy;
                entry.Entity.LastModified = DateTime.UtcNow;
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
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}