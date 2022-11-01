using fh_service_directory_api.core.Extensions;

namespace fh_service_directory_api.api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryClient(configuration, "fh_service_directory_api.api");
            services.AddApplicationInsightsServiceName(configuration, "fh_service_directory_api.api");
            services.AddLogging("fh_service_directory_api.api");
            services.AddTelemetry();
        }
    }
}
