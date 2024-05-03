namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalMetricsEndPoints
{
    public void RegisterMetricsEndPoints(WebApplication app)
    {
        app.MapPost("api/metrics/service-search", (IConfiguration configuration, ILogger<MinimalMetricsEndPoints> logger) =>
        {
            try
            {
                ///
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred reporting metric data. {ExceptionMessage}", ex.Message);
                throw;
            }
        });
    }
}
