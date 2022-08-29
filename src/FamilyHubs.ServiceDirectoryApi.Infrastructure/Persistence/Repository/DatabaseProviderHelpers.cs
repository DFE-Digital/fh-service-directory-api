using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Repository
{
    public static class DatabaseProviderHelpers
    {
        public static bool IsRealDatabase(this DbContext context)
        {
            if (context != null && context.Database != null && context.Database.ProviderName != null)
                return context.Database.ProviderName.Contains("SqlServer");
            return false;
        }
    }
}
