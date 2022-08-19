using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.infrastructure.Persistence.Repository
{
    public static class DatabaseProviderHelpers
    {
        public static bool IsRealDatabase(this DbContext context)
        {
            return context.Database.ProviderName.Contains("SqlServer");
        }
    }
}
