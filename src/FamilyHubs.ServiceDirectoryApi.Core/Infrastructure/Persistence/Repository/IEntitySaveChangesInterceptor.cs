using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Persistence.Repository
{
    public interface IEntitySaveChangesInterceptor
    {
        InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result);
        ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default);
        void UpdateEntities(DbContext? context);
    }
}